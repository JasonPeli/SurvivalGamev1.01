using System.Collections;
using UnityEngine;
using System.Linq;

public class InteractBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private Inventory inventory;
    private Vector3 spawnItemOffset = new Vector3(0, 0.5f, 0);
    [HideInInspector]
    public bool isBusy = false;
    [SerializeField]
    private Equipment equipmentSystem;
    [SerializeField]
    private EquipmentLibrary equipmentLibrary;
    [SerializeField]
    private AudioSource audioSource;


    [Header("Tools Configuration")]

    [SerializeField]
    private GameObject pickaxeVisual;
    [SerializeField]
    private AudioClip pickaxeSound;
    [SerializeField]
    private GameObject axeVisual;
    [SerializeField]
    private AudioClip axeSound;
    [SerializeField]
    private GameObject woodVisual;

    [Header("Others")]
    [SerializeField]
    private AudioClip pickupSound;




    private Item currentItem;
    private Harvestable currentHarvestable;
    private Tool currentTool;


    public void DoPickup(Item item)
    {
        if (isBusy)
        {
            return;
        }
        isBusy = true;
        if (inventory.IsFull())
        {
            Debug.Log("Inventory is full");
            return;
        }
        currentItem = item;
        playerAnimator.SetTrigger("Pickup");
        playerMoveBehaviour.canMove = false;
    }

    public void DoHarvest(Harvestable harvestable)
    {
        if (isBusy)
        {
            return;
        }

        // Vérifie si le joueur a l'item requis dans l'inventaire
        Debug.Log("Tentative de récolte avec item requis : " + harvestable.requiredItem.name);
        if (!inventory.HasItem(harvestable.requiredItem))
        {
            Debug.Log("Le joueur n'a pas l'item requis pour cette action.");
            return;
        }

        isBusy = true;

        currentTool = harvestable.requiredItem.tool;
        EnableToolGameObjectFromEnum(currentTool);
        currentHarvestable = harvestable;
        playerAnimator.SetTrigger("Harvest");
        playerMoveBehaviour.canMove = false;
    }
    // Coroutine appeler depuis l'animation harvesting
    IEnumerator BreakHarvestable()
    {
        Harvestable currentlyHarvesting = currentHarvestable;
        //permet de désacttiver le layer harvestable pour ne plus pouvoir interagir avec
        currentlyHarvesting.gameObject.layer = LayerMask.NameToLayer("Default");
        if (currentlyHarvesting.disableKinematicOnHarvest)
        {
            Rigidbody rigidbody = currentlyHarvesting.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(transform.forward * 800, ForceMode.Impulse);

        }

        yield return new WaitForSeconds(currentlyHarvesting.destroyDelay);

        for (int i = 0; i < currentlyHarvesting.harvestableItems.Length; i++)
        {
            Ressource ressource = currentlyHarvesting.harvestableItems[i];

            if (Random.Range(1, 101) <= ressource.dropChance)
            {
                GameObject instantiatedRessource = Instantiate(ressource.itemData.prefab);
                instantiatedRessource.transform.position = currentlyHarvesting.transform.position + spawnItemOffset;
            }
        }
        Destroy(currentlyHarvesting.gameObject);
    }

    public void AddItemToInventory()
    {
        inventory.AddItem(currentItem.itemData);
        audioSource.PlayOneShot(pickupSound);
        Destroy(currentItem.gameObject);
    }

    public void ReEnablePlayerMovement()
    {
        EnableToolGameObjectFromEnum(currentTool, false);
        playerMoveBehaviour.canMove = true;
        isBusy = false;

    }
    private void EnableToolGameObjectFromEnum(Tool toolType, bool enabled = true)
    {

        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == equipmentSystem.equipedWeaponItem).FirstOrDefault();

        if (equipmentLibraryItem != null)
        {
            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(enabled);
            }

            equipmentLibraryItem.itemprefab.SetActive(!enabled);
        }

        switch (toolType)
        {
            case Tool.Pickaxe:
                pickaxeVisual.SetActive(enabled);
                audioSource.clip = pickaxeSound;
                break;
            case Tool.Axe:
                axeVisual.SetActive(enabled);
                audioSource.clip = axeSound;
                break;
            case Tool.Wood:
                woodVisual.SetActive(enabled);
                break;
        }
    }

    public void PlayHarvestingSoundEffect()
    {
        audioSource.Play();
    }

    public void DoInteractWithChest(Chest chest)
    {
        if (isBusy)
        {
            return;
        }
        isBusy = true;
        chest.Interact();
        playerMoveBehaviour.canMove = false;
    }

}

