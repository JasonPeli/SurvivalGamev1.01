using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{

    [Header ("other elements references")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private MoveBehaviour playerMovementScript;


    [Header("Health")]

    [SerializeField]
    private float maxHealth = 100f;
    public float currentHealth;

    [SerializeField]
    private Image healthBarFill;
    [SerializeField]
    private float healthDecreaseRateForHungerAndThirst;

    [Header("Hunger")]

    [SerializeField]
    private float maxHunger = 100f;

    public float currentHunger;
    [SerializeField]
    private Image hungerBarFill;
    [SerializeField]
    private float hungerDecreaseRate;

    [Header("Thirst")]

    [SerializeField]
    private float maxThirst = 100f;
    public float currentThirst;
    [SerializeField]
    private Image thirstBarFill;
    [SerializeField]
    private float thirstDecreaseRate;
    public float currentArmorpoints;

    [HideInInspector]
    public bool isDead = false;
    void Awake()
    {
        currentHealth = maxHealth; 
        currentHunger = maxHunger;
        currentThirst = maxThirst;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHungerAndThirstBarFill();
        if(Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(50);
        }
    }

    public void ConsumeItem(float health, float hunger, float thirst)
    {
        currentHealth += health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }


        currentHunger += hunger;
        if (currentHunger > maxHunger)
        {
            currentHunger = maxHunger;
        }


        currentThirst += thirst;
        if (currentThirst > maxThirst)
        {
            currentThirst = maxThirst;
        }

        UpdateHealthBarFill();
    }
    public void TakeDamage(float damage, bool overtime = false)
    {
        if (overtime)
        {
            currentHealth -= damage * Time.deltaTime;
        }
        else
        {
            currentHealth -= damage * (1 - (currentArmorpoints / 100));
        }
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
        UpdateHealthBarFill();
    }
    private void Die()
    {
        Debug.Log("You are dead");
        isDead = true;


        playerMovementScript.canMove = false;

        animator.SetTrigger("Die");
    }
    void UpdateHealthBarFill()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }
    void UpdateHungerAndThirstBarFill()
    {
        // Diminue la faim/soif au fil du temps
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentThirst -= thirstDecreaseRate * Time.deltaTime;


        // On empêche de passer dans le négatif:
        currentHunger = currentHunger < 0 ? 0 : currentHunger;
        currentThirst = currentThirst < 0 ? 0 : currentThirst;
        //Metre a jour les visuels
        hungerBarFill.fillAmount = currentHunger / maxHunger;
        thirstBarFill.fillAmount = currentThirst / maxThirst;

        //Si la barre de faim et/ou soif est vide, le joueur perd de la vie
        if(currentHunger <= 0 || currentThirst <= 0)
        {
            TakeDamage((currentHunger <= 0 && currentThirst <= 0 ? healthDecreaseRateForHungerAndThirst * 2 : healthDecreaseRateForHungerAndThirst ), true);
        }
    }
}
