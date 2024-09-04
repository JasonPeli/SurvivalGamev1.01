using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    [Header(" OTHER SCRIPTS REFERENCES")]
    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    [SerializeField]
    private PlayerStats playerStats;

    [Header("EQUIPMENT SYSTEM VARIABLES")]

    [SerializeField]
    private EquipmentLibrary equipmentLibrary;
    [SerializeField]
    private Image headSlotImage;
    [SerializeField]
    private Image chestSlotImage;
    [SerializeField]
    private Image handsSlotImage;
    [SerializeField]
    private Image legsSlotImage;
    [SerializeField]
    private Image feetSlotImage;

    [SerializeField]
    private Image weaponSlotImage;
    // Garde une trace des équipements actuels : 
    private ItemData equipedHeadItem;
    private ItemData equipedChestItem;
    private ItemData equipedHandsItem;
    private ItemData equipedLegsItem;
    private ItemData equipedFeetItem;
    [HideInInspector]
    public ItemData equipedWeaponItem;

    [SerializeField]
    private Button headSlotDesequipButton;
    [SerializeField]
    private Button chestSlotDesequipButton;
    [SerializeField]
    private Button handsSlotDesequipButton;
    [SerializeField]
    private Button legsSlotDesequipButton;
    [SerializeField]
    private Button feetSlotDesequipButton;
    [SerializeField]
    private Button weaponSlotDesequipButton;

    [Header("AUDIO")]
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip equipSound;
    private void DisablePreviousEquipedEquipment(ItemData itemToDisable)
    {
        if (itemToDisable == null)
        {
            return;
        }

        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.FirstOrDefault(elem => elem.itemData == itemToDisable);

        if (equipmentLibraryItem != null)
        {
            foreach (var element in equipmentLibraryItem.elementsToDisable)
            {
                if (element != null) element.SetActive(true);
            }
            if (equipmentLibraryItem.itemprefab != null) equipmentLibraryItem.itemprefab.SetActive(false);
        }

        playerStats.currentArmorpoints -= itemToDisable.armorPoints;
        Inventory.instance.AddItem(itemToDisable);
    }



    public void DesequipEquipment(EquipmentType equipmentType)
    {
        if (Inventory.instance.IsFull())
        {
            Debug.Log("L'inventaire est plein, impossible de se déséquiper de cet élément");
            return;
        }
        ItemData currentItem = null;

        switch (equipmentType)
        {
            case EquipmentType.Head:
                currentItem = equipedHeadItem;
                equipedHeadItem = null;
                headSlotImage.sprite = Inventory.instance.emptySprite;
                break;

            case EquipmentType.Chest:
                currentItem = equipedChestItem;
                equipedChestItem = null;
                chestSlotImage.sprite = Inventory.instance.emptySprite;
                break;

            case EquipmentType.Hands:
                currentItem = equipedHandsItem;
                equipedHandsItem = null;
                handsSlotImage.sprite = Inventory.instance.emptySprite;
                break;

            case EquipmentType.Legs:
                currentItem = equipedLegsItem;
                equipedLegsItem = null;
                legsSlotImage.sprite = Inventory.instance.emptySprite;
                break;

            case EquipmentType.Feet:
                currentItem = equipedFeetItem;
                equipedFeetItem = null;
                feetSlotImage.sprite = Inventory.instance.emptySprite;
                break;
            case EquipmentType.Weapon:
                currentItem = equipedWeaponItem;
                equipedWeaponItem = null;
                weaponSlotImage.sprite = Inventory.instance.emptySprite;
                break;
        }
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == currentItem).First();
        if (equipmentLibraryItem != null)
        {
            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(true);
            }
            equipmentLibraryItem.itemprefab.SetActive(false);
        }

        playerStats.currentArmorpoints -= currentItem.armorPoints;

        Inventory.instance.AddItem(currentItem);
        Inventory.instance.RefreshContent();
    }
    // public void UpdateEquipmentsDesequipButtons()
    // {
    //     headSlotDesequipButton.onClick.RemoveAllListeners();
    //     headSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Head); });
    //     headSlotDesequipButton.gameObject.SetActive(equipedHeadItem);

    //     chestSlotDesequipButton.onClick.RemoveAllListeners();
    //     chestSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Chest); });
    //     chestSlotDesequipButton.gameObject.SetActive(equipedChestItem);

    //     handsSlotDesequipButton.onClick.RemoveAllListeners();
    //     handsSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Hands); });
    //     handsSlotDesequipButton.gameObject.SetActive(equipedHandsItem);

    //     legsSlotDesequipButton.onClick.RemoveAllListeners();
    //     legsSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Legs); });
    //     legsSlotDesequipButton.gameObject.SetActive(equipedLegsItem);

    //     feetSlotDesequipButton.onClick.RemoveAllListeners();
    //     feetSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Feet); });
    //     feetSlotDesequipButton.gameObject.SetActive(equipedFeetItem);

    //     weaponSlotDesequipButton.onClick.RemoveAllListeners();
    //     weaponSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Weapon); });
    //     weaponSlotDesequipButton.gameObject.SetActive(equipedWeaponItem);
    // }
    public void EquipAction(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogError("Equipment: itemData is null in EquipAction");
            return;
        }

        Debug.Log("Equipment: Equipping item " + itemData.name);

        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.FirstOrDefault(elem => elem.itemData == itemData);

        if (equipmentLibraryItem == null)
        {
            Debug.LogError("Equipment: Item " + itemData.name + " not found in equipment library");
            return;
        }

        switch (itemData.equipmentType)
        {
            case EquipmentType.Head:
                DisablePreviousEquipedEquipment(equipedHeadItem);
                headSlotImage.sprite = itemData.visual;
                equipedHeadItem = itemData;
                break;
            case EquipmentType.Chest:
                DisablePreviousEquipedEquipment(equipedChestItem);
                chestSlotImage.sprite = itemData.visual;
                equipedChestItem = itemData;
                break;
            case EquipmentType.Hands:
                DisablePreviousEquipedEquipment(equipedHandsItem);
                handsSlotImage.sprite = itemData.visual;
                equipedHandsItem = itemData;
                break;
            case EquipmentType.Legs:
                DisablePreviousEquipedEquipment(equipedLegsItem);
                legsSlotImage.sprite = itemData.visual;
                equipedLegsItem = itemData;
                break;
            case EquipmentType.Feet:
                DisablePreviousEquipedEquipment(equipedFeetItem);
                feetSlotImage.sprite = itemData.visual;
                equipedFeetItem = itemData;
                break;
            case EquipmentType.Weapon:
                DisablePreviousEquipedEquipment(equipedWeaponItem);
                weaponSlotImage.sprite = itemData.visual;
                equipedWeaponItem = itemData;
                break;
            default:
                Debug.LogError("Equipment: Unknown equipment type " + itemData.equipmentType);
                break;
        }

        for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
        {
            equipmentLibraryItem.elementsToDisable[i].SetActive(false);
        }

        equipmentLibraryItem.itemprefab.SetActive(true);
        playerStats.currentArmorpoints += itemData.armorPoints;

        Inventory.instance.RemoveItem(itemData);
        Debug.Log("Equipment: Equipped item " + itemData.name);
    }


    public void UpdateEquipmentsDesequipButtons()
    {
        UpdateButtonState(headSlotDesequipButton, equipedHeadItem, EquipmentType.Head);
        UpdateButtonState(chestSlotDesequipButton, equipedChestItem, EquipmentType.Chest);
        UpdateButtonState(handsSlotDesequipButton, equipedHandsItem, EquipmentType.Hands);
        UpdateButtonState(legsSlotDesequipButton, equipedLegsItem, EquipmentType.Legs);
        UpdateButtonState(feetSlotDesequipButton, equipedFeetItem, EquipmentType.Feet);
        UpdateButtonState(weaponSlotDesequipButton, equipedWeaponItem, EquipmentType.Weapon);
    }

    private void UpdateButtonState(Button button, ItemData equippedItem, EquipmentType type)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { DesequipEquipment(type); });
        button.gameObject.SetActive(equippedItem != null);
    }
}
