using UnityEngine;

public class ChestRaycastInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayer; // Définis ici les layers avec lesquels le raycast doit interagir (p. ex. le layer du joueur)
    [SerializeField] private float interactionDistance = 2f; // Distance maximale du raycast

    private Chest chest;
    private Camera mainCamera;

    private void Start()
    {
        chest = GetComponent<Chest>();
        mainCamera = Camera.main; // Assure-toi d'avoir une caméra principale dans la scène
    }

    private void Update()
    {
        // Vérifie si le joueur appuie sur une touche pour interagir (par exemple E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Lance un raycast depuis la caméra vers l'avant (direction du joueur)
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Déclaration d'une variable RaycastHit pour recevoir les informations du raycast
            RaycastHit hit;

            // Effectue le raycast et vérifie s'il a touché quelque chose
            if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
            {
                // Vérifie si ce que le raycast a touché est le coffre
                if (hit.collider.gameObject == gameObject)
                {
                    // Appelle la fonction Interact() du script Chest
                    chest.Interact();
                }
            }
        }
    }
}
