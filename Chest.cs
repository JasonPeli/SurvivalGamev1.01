using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private List<ItemInInventory> chestContent = new List<ItemInInventory>();
    [SerializeField] private GameObject chestUIPanel;
    [SerializeField] private Transform chestSlotsParent;
    [SerializeField] private GameObject chestSlotPrefab;
    [SerializeField] public Sprite emptySprite;

    private bool isOpen = false;

    public void Interact()
    {
        isOpen = !isOpen;
        chestUIPanel.SetActive(isOpen);

        if (isOpen)
        {
            UpdateChestUI();
        }
    }

    private void UpdateChestUI()
    {
        for (int i = 0; i < chestSlotsParent.childCount; i++)
        {
            Slot currentSlot = chestSlotsParent.GetChild(i).GetComponent<Slot>();
            if (i < chestContent.Count)
            {
                currentSlot.SetItemData(chestContent[i].itemData, chestContent[i].count);
            }
            else
            {
                currentSlot.SetItemData(null, 0); // Initialise même les slots vides
            }
            currentSlot.Initialize(this); // Initialise avec une référence à ce coffre
        }
    }

    public void DropItem(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogError("Chest: Trying to drop null item.");
            return;
        }

        AdjustItemQuantity(itemData, -1);
        Debug.Log("Item dropped: " + itemData.name);
        UpdateChestUI();
    }

    public void DestroyItem(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogError("Chest: Trying to destroy null item.");
            return;
        }

        AdjustItemQuantity(itemData, -1);
        Debug.Log("Item destroyed: " + itemData.name);
        UpdateChestUI();
    }

    public void EquipItem(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogError("Chest: itemData is null in EquipItem");
            return;
        }

        Debug.Log("Chest: Equipping item " + itemData.name);

        AdjustItemQuantity(itemData, -1);

        Debug.Log("Item equipped: " + itemData.name);

        UpdateChestUI();
    }

    public void TransferToChest(ItemData itemData)
    {
        if (itemData != null)
        {
            Debug.Log("Current inventory before removing: " + Inventory.instance.GetContent());
            Inventory.instance.RemoveItem(itemData);
            Debug.Log("Current inventory after removing: " + Inventory.instance.GetContent());

            AddItem(itemData);
            Debug.Log("Transferred " + itemData.name + " to chest.");
            UpdateChestUI();
        }
        else
        {
            Debug.LogWarning("TransferToChest: itemData is null.");
        }
    }


    private void AdjustItemQuantity(ItemData item, int amount)
    {
        var itemInChest = chestContent.Find(i => i.itemData == item);

        if (itemInChest == null)
        {
            Debug.LogError("Chest: item not found in chest content");
            return;
        }

        Debug.Log("Chest: Adjusting quantity of item " + item.name + " by " + amount);

        itemInChest.count += amount;

        if (itemInChest.count <= 0)
        {
            Debug.Log("Chest: Removing item " + item.name + " from chest");
            chestContent.Remove(itemInChest);
        }
    }

    private void AddItem(ItemData item)
    {
        var itemInChest = chestContent.Find(i => i.itemData == item);

        if (itemInChest != null)
        {
            itemInChest.count++;
        }
        else
        {
            chestContent.Add(new ItemInInventory { itemData = item, count = 1 });
        }
    }
}
