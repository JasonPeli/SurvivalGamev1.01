using UnityEngine;

public class ItemActionsSystem : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]
    [SerializeField]
    private Equipment equipment;
    [SerializeField]
    private PlayerStats playerStats;

    [Header("ITEM ACTIONS SYSTEM VARIABLES")]
    [SerializeField]
    public GameObject actionPanel;
    [SerializeField]
    private Transform dropPoint;
    [SerializeField]
    private GameObject useItemButton;
    [SerializeField]
    private GameObject dropItemButton;
    [SerializeField]
    private GameObject equipItemButton;
    [SerializeField]
    private GameObject destroyItemButton;

    [HideInInspector]
    public ItemData itemCurrentlySelected;

    private Chest currentChest;

    public void OpenActionPanel(ItemData item, Vector3 slotPosition, Chest chest = null)
    {
        itemCurrentlySelected = item;
        currentChest = chest;

        if (item == null)
        {
            Debug.LogError("ItemActionsSystem: No item provided");
            actionPanel.SetActive(false);
            return;
        }

        Debug.Log("ItemActionsSystem: Opening action panel for item " + item.name);

        switch (item.itemType)
        {
            case ItemType.Ressource:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(false);
                break;
            case ItemType.Equipment:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(true);
                break;
            case ItemType.Consumable:
                useItemButton.SetActive(true);
                equipItemButton.SetActive(false);
                break;
        }

        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);
    }


    public void CloseActionPanel()
    {
        actionPanel.SetActive(false);
        itemCurrentlySelected = null;
        currentChest = null;
    }

    public void UseActionButton()
    {
        playerStats.ConsumeItem(itemCurrentlySelected.healthEffect, itemCurrentlySelected.hungerEffect, itemCurrentlySelected.thirstEffect);
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        if (currentChest != null)
        {
            currentChest.DestroyItem(itemCurrentlySelected);
        }
        CloseActionPanel();
    }

    public void EquipActionButton()
    {
        if (equipment == null)
        {
            Debug.LogError("ItemActionsSystem: equipment is null");
            return;
        }

        if (itemCurrentlySelected == null)
        {
            Debug.LogError("ItemActionsSystem: itemCurrentlySelected is null");
            return;
        }

        Debug.Log("ItemActionsSystem: Equipping item " + itemCurrentlySelected.name);

        equipment.EquipAction(itemCurrentlySelected);

        Inventory.instance.RemoveItem(itemCurrentlySelected);

        if (currentChest != null)
        {
            Debug.Log("ItemActionsSystem: Removing item from chest " + currentChest.name);
            currentChest.EquipItem(itemCurrentlySelected);
        }
        else
        {
            Debug.LogWarning("ItemActionsSystem: currentChest is null, item is not in a chest");
        }

        CloseActionPanel();
    }




    public void DropActionButton()
    {
        GameObject instantiatedItem = Instantiate(itemCurrentlySelected.prefab);
        instantiatedItem.transform.position = dropPoint.position;
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        if (currentChest != null)
        {
            currentChest.DropItem(itemCurrentlySelected);
        }
        Inventory.instance.RefreshContent();
        CloseActionPanel();
    }

    public void DestroyActionButton()
    {
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        if (currentChest != null)
        {
            currentChest.DestroyItem(itemCurrentlySelected);
        }
        Inventory.instance.RefreshContent();
        CloseActionPanel();
    }

    private void Start()
    {
        if (equipment == null)
        {
            Debug.LogError("ItemActionsSystem: equipment is not assigned in the inspector");
        }

        if (Inventory.instance == null)
        {
            Debug.LogError("ItemActionsSystem: Inventory.instance is not initialized");
        }
    }
}

