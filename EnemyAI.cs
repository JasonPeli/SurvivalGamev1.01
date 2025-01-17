using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AudioSource audioSource;
    private Transform player;
    private PlayerStats playerStats;

    [Header ("Enemy Stats")]

    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float detectionRadius;

    [SerializeField]
    private float walkspeed;
    [SerializeField]
    private float chasespeed;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private float attackDelay;
    [SerializeField]
    private float damageDealt;
    [SerializeField]
    private float rotationSpeed;

    [Header("Wandering parameters")]
    [SerializeField]
    private float wanderingWaitTimeMin;
    [SerializeField]
    private float wanderingWaitTimeMax;

    [SerializeField]
    private float wanderingDistanceMin;
    [SerializeField]
    private float wanderingDistanceMax;
    private bool hasDestination;
    private bool isAttacking;
    private bool isDead;

    [Header("Loot Drop")]
    [SerializeField]
    private GameObject[] itemsToDrop;
    [SerializeField]
    private float[] dropChances;

private void Awake()
{
    Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    player = playerTransform;
    playerStats = playerTransform.GetComponent<PlayerStats>();

    currentHealth = maxHealth;
}

    void Update()
    {
        if(Vector3.Distance(player.position,transform.position) <= detectionRadius && !playerStats.isDead)
        {
            agent.speed = chasespeed;

            Quaternion rot = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot,rotationSpeed * Time.deltaTime);

            if(!isAttacking)
            {
                if (Vector3.Distance(player.position,transform.position) <= attackRadius)
                {
                    StartCoroutine(AttackPlayer());
                }
                else
                {
                    agent.SetDestination(player.position);
                }
            }
            
        }
        else
        {
            agent.speed = walkspeed;
            if(agent.remainingDistance <0.75f && !hasDestination)
            {
                StartCoroutine(GetNewDestination());
            }
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    public void TakeDamage(float damages)
    {
        if(isDead)
        {
            return;
        }



        currentHealth -= damages;
        if(currentHealth <= 0)
        {
            isDead = true;
            animator.SetTrigger("Die");
            agent.enabled = false;
            DropItems();
            enabled = false;
        }
        else
        {
            animator.SetTrigger("GetHit");
        }
    }

    void DropItems()
    {
        for (int i = 0; i < itemsToDrop.Length; i++)
        {
            float randomValue = Random.Range(0f, 100f);
            if (randomValue <= dropChances[i])
            {
                Instantiate(itemsToDrop[i], transform.position, Quaternion.identity);
            }
        }
    }
    IEnumerator GetNewDestination()
    {
        hasDestination = true;
        yield return new WaitForSeconds(Random.Range(wanderingWaitTimeMin, wanderingWaitTimeMax));
        
        
        Vector3 nextDestination = transform.position;
        nextDestination += Random.Range(wanderingDistanceMin,wanderingDistanceMax) * new Vector3(Random.Range(-1f,1f),0f,Random.Range(-1f,1f)).normalized;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(nextDestination, out hit, wanderingDistanceMax, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        hasDestination = false;
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;

        audioSource.Play();
        
        animator.SetTrigger("Attack");
        playerStats.TakeDamage(damageDealt);

        yield return new WaitForSeconds(attackDelay);
        if(agent.enabled)
        {
            agent.isStopped = false;
        }
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
