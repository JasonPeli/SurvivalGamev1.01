using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]
    [SerializeField] private Equipment equipment;
    [SerializeField] public ItemActionsSystem itemActionsSystem;
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private Chest chest;

    public static Inventory instance { get; private set; }

    [Header("INVENTORY SYSTEM VARIABLES")]
    private List<ItemInInventory> content = new List<ItemInInventory>();
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private Transform inventorySlotsParent;
    public Sprite emptySprite;

    const int InventorySize = 18;
    private bool isOpen = false;

    [SerializeField] private List<Tool> tools = new List<Tool>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Définit l'instance unique de l'inventaire
        }
        else
        {
            Debug.LogWarning("Multiple instances of Inventory found!");
            Destroy(gameObject); // Détruit les instances supplémentaires
        }
    }

    public List<ItemInInventory> GetContent()
    {
        return content;
    }

    private void Start()
    {
        CloseInventory();
        RefreshContent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    

    public void OnEndDragAction(Transform draggedTransform)
    {
        Debug.Log("OnEndDragAction called with " + draggedTransform.name);

        if (chest == null)
        {
            Debug.LogError("Chest is not assigned in the inspector!");
            return;
        }

        Slot slotComponent = draggedTransform.GetComponentInParent<Slot>();

        if (slotComponent != null)
        {

            ItemData itemData = slotComponent.item;

            if (itemData != null)
            {
                Debug.Log("ItemData found: " + itemData.name);
                chest.TransferToChest(itemData);
            }
            else
            {
                Debug.LogWarning("Slot does not have a public ItemData variable.");
            }
        }
        else
        {
            Debug.LogWarning("OnEndDragAction: Slot component not found in parent.");
        }
    }

    public bool HasItem(ItemData requiredItem)
    {
        return content.Any(itemInInventory => itemInInventory.itemData == requiredItem);
    }

    public void AddItem(ItemData item)
    {
        Debug.Log("Adding item: " + item.name);

        ItemInInventory existingItem = content.FirstOrDefault(elem => elem.itemData == item);

        if (existingItem != null && item.stackable)
        {
            if (existingItem.count < item.maxStack)
            {
                existingItem.count++;
            }
        }
        else
        {
            content.Add(new ItemInInventory { itemData = item, count = 1 });
        }

        UnlockRecipesContainingItem(item);
        RefreshContent();
    }

    private void UnlockRecipesContainingItem(ItemData item)
    {
        RecipeData[] recipes = craftingSystem.GetAvailableRecipes();
        bool newRecipeUnlocked = false;

        foreach (var recipe in recipes)
        {
            if (recipe.requiredItems.Any(requiredItem => requiredItem.itemData == item))
            {
                if (!craftingSystem.IsRecipeUnlocked(recipe))
                {
                    craftingSystem.UnlockRecipe(recipe);
                    newRecipeUnlocked = true;
                }
            }
        }

        if (newRecipeUnlocked)
        {
            uiManager.ShowEphemereMessage("Nouvelle recette débloquée !");
        }
    }

    public void RemoveItem(ItemData itemData)
    {
        var itemInInventory = content.Find(i => i.itemData == itemData);

        if (itemInInventory != null)
        {
            if (itemInInventory.count > 1)
            {
                itemInInventory.count--;
            }
            else
            {
                content.Remove(itemInInventory);
            }
        }
        else
        {
            Debug.LogWarning("Attempted to remove an item that does not exist in the inventory: " + itemData.name);
        }
    }



    private void OpenInventory()
    {
        InventoryPanel.SetActive(true);
        isOpen = true;
    }

    public void CloseInventory()
    {
        InventoryPanel.SetActive(false);
        itemActionsSystem.actionPanel.SetActive(false);
        TooltipSystem.instance.Hide();
        isOpen = false;
    }

    public void RefreshContent()
    {
        Debug.Log("Refreshing inventory content...");

        // Réinitialise les slots visuels
        for (int i = 0; i < inventorySlotsParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptySprite;
            currentSlot.countText.enabled = false;
        }

        // Peuple les slots avec le contenu actuel de l'inventaire
        for (int i = 0; i < content.Count; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = content[i].itemData;
            currentSlot.itemVisual.sprite = content[i].itemData.visual;

            if (currentSlot.item.stackable)
            {
                currentSlot.countText.enabled = true;
                currentSlot.countText.text = content[i].count.ToString();
            }

            // Ajoute ou met à jour le gestionnaire de glissement des objets d'inventaire
            ItemDragHandler dragHandler = currentSlot.GetComponent<ItemDragHandler>();
            if (dragHandler == null)
            {
                dragHandler = currentSlot.gameObject.AddComponent<ItemDragHandler>();
            }

            dragHandler.OnEndDragAction = OnEndDragAction;
        }

        equipment.UpdateEquipmentsDesequipButtons();
        craftingSystem.UpdateDisplayedRecipes();
    }

    public bool IsFull()
    {
        return content.Count >= InventorySize;
    }
}

[System.Serializable]
public class ItemInInventory
{
    public ItemData itemData;
    public int count;
}
