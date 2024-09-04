using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float checkRadius;
    [SerializeField]
    private LayerMask groundLayers;
    [SerializeField]
    private float minimalHeightForDamages;
    [SerializeField]
    private PlayerStats currentHealth;

    private bool grounded;
    private float previousHeight;

    void Update()
    {
        bool previouslyGrounded = grounded;
        grounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayers);

        if (grounded)
        {
            float fallHeight = previousHeight - groundCheck.position.y;

            if (!previouslyGrounded && fallHeight > minimalHeightForDamages)
            {
                DoFallDamage(fallHeight);
            }

            previousHeight = groundCheck.position.y;
        }
    }

    void DoFallDamage(float fallHeight)
    {
        if (fallHeight <= 2)
        {
            Debug.Log("Chute de - de 2 mètres");
        }
        else if (fallHeight <= 3)
        {
            Debug.Log("Chute de - de 3 mètres");
            currentHealth.TakeDamage(10); // Applique des dégâts pour une chute de plus de 2 mètres mais moins de 3
        }
        else if (fallHeight <=4)
        {
            Debug.Log("Chute de - de 4 mètres");
            currentHealth.TakeDamage(15); // Applique des dégâts pour une chute de plus de 3 mètres mais moins de 4
        }
        else if (fallHeight <= 5)
        {
            Debug.Log("Chute de - de 5 mètres");
            currentHealth.TakeDamage(20); // Applique des dégâts pour une chute de plus de 4 mètres mais moins de 5
        }
        else if (fallHeight <= 6)
        {
            Debug.Log("Chute de - de 6 mètres");
            currentHealth.TakeDamage(25); // Applique des dégâts pour une chute de plus de 5 mètres mais moins de 6
        }
    }
}
