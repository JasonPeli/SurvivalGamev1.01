using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField]
    private float interactRange = 2.6f;

    public InteractBehaviour playerInteractBehaviour;

    [SerializeField]
    private GameObject interactText;

    [SerializeField]
    private LayerMask layerMask;

    void Update()
{
    RaycastHit hit;

    if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange, layerMask))
    {
        interactText.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hit.transform.CompareTag("Item"))
            {
                playerInteractBehaviour.DoPickup(hit.transform.gameObject.GetComponent<Item>());
            }
            else if (hit.transform.CompareTag("Harvestable"))
            {
                playerInteractBehaviour.DoHarvest(hit.transform.gameObject.GetComponent<Harvestable>());
            }
            else if (hit.transform.CompareTag("Chest"))
            {
                Debug.Log("Interacting with chest!");
                Chest chest = hit.transform.GetComponent<Chest>();
                if (chest != null)
                {
                    chest.Interact(); // Assure-toi que cette fonction est appelée
                }
            }
        }
    }
    else
    {
        interactText.SetActive(false);
    }
}
}
