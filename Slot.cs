using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData item;
    public Image itemVisual;
    public Text countText;
    public GameObject slotBackground;
    public Button slotButton;

     public ItemData itemData { get; private set; }

    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    private Chest chest;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            TooltipSystem.instance.Show(item.description, item.name);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.instance.Hide();
    }

    public void ClicOnSlot()
    {
        itemActionsSystem.OpenActionPanel(item, transform.position, chest);
    }

    public void Initialize(Chest chest)
    {
        this.chest = chest;
    }

    public void SetItemData(ItemData itemData, int count)
    {
        item = itemData;
        if (itemData != null)
        {
            itemVisual.sprite = itemData.visual;
            itemVisual.enabled = true;
            countText.text = count > 1 ? count.ToString() : "";
            countText.enabled = true;
        }
        else
        {
            itemVisual.sprite = null; // Assure-toi de remettre l'image par défaut
            itemVisual.enabled = false;
            countText.text = "";
            countText.enabled = false;
        }
        slotBackground.SetActive(true); // Toujours actif
        slotButton.interactable = true; // Toujours interactif
    }
}