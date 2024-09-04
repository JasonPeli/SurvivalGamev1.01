using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Equipment equipmentSystem;
    [SerializeField]
    private UiManager uiManager;
    [SerializeField]
    private InteractBehaviour interactBehaviour;
    [Header("configuration")]
    private bool isAttacking;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private Vector3 attackOffset;
 
    void Update()
    {
        // Debug.DrawRay(transform.position + attackOffset, transform.forward * attackRange, Color.red, 0.1f);
        
        
        if(Input.GetMouseButtonDown(0) && CanAttack())
        {
            isAttacking = true;
            SendAttack();
            animator.SetTrigger("Attack");
        }
    }

    void SendAttack()
    {
        Debug.Log("Attack");

        RaycastHit hit;

        if(Physics.Raycast(transform.position + attackOffset, transform.forward, out hit, attackRange, layerMask))
        {
            if(hit.transform.CompareTag("AI"))
            {
                Debug.Log("Hit AI");
                EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
                enemy.TakeDamage(equipmentSystem.equipedWeaponItem.attackPoints);
            }
        }
    }

    bool CanAttack()
    {
        // Pour attaquer on doit : 
        // avoir une arme équiper & ne pas être en train d'attaquer
        // ne pas avoir l'inventaire ouvert
        return equipmentSystem.equipedWeaponItem != null && ! isAttacking && !uiManager.atLeastOnePanelOpened && !interactBehaviour.isBusy;
    }
    
    public void AttackFinished()
    {
        isAttacking = false;
    }
}
