using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private int maxHealth;
    [SerializeField] private List<CardData> startingDeck;
    [SerializeField] private int handSize;
    [SerializeField] private int money;
    [SerializeField] private int maxFear;

    [SerializeField] private float healthRegeneration;

    private int currentHealth;
    private int currentFear;
    private int currentDefense;
    private int cardsToDraw;


    private void Start()
    {
        maxHealth = 30;
        handSize = 5;
        money = 100;
        maxFear = 5;

        startingDeck ??= CreateStartingDeck();

        healthRegeneration = 0.3f;
        currentHealth = maxHealth;
        currentFear = maxFear;
        currentDefense = 0;
        cardsToDraw = 0;
    }

    // ********** HEALTH **********
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= ApplyDefense(damage);

        currentHealth = Mathf.Max(currentHealth, 0);
    }

    public void Heal(int health)
    {
        currentHealth += health;

        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    public void IncreaseMaxHealth(int increment)
    {
        maxHealth += increment;
        currentHealth += increment;
    }


    // ********** FEAR **********
    public int GetMaxFear()
    {
        return maxFear;
    }
    
    public int GetCurrentFear()
    {
        return currentFear;
    }

    public void IncreaseCurrentFear(int amount)
    {
        currentFear += amount;
    }

    public void DecreaseCurrentFear(int amount)
    {
        currentFear -= amount;

        currentFear = Mathf.Max(currentFear, 0);
    }

    public void IncreaseMaxFear(int amount)
    {
        maxFear += amount;
    }

    public void ResetFear()
    {
        currentFear = maxFear;
    }

    // ********** MONEY **********
    public int GetMoney()
    {
        return money;
    }

    public void IncreaseMoney(int amount)
    {
        money += amount;
    }

    public void DecreaseMoney(int amount)
    {
        money -= amount;

        money = Mathf.Max(money, 0);
    }


    // ********** DEFENSE **********
    public int GetDefense()
    {
        return currentDefense;
    }

    public void IncreaseDefense(int amount)
    {
        currentDefense += amount;
    }

    public void ResetDefense()
    {
        currentDefense = 0;
    }

    private int ApplyDefense(int damage)
    {
        int reduction = Mathf.Min(currentDefense, damage);
        currentDefense -= reduction;

        return damage - reduction;
    }


    // ********** DECK **********
    private List<CardData> CreateStartingDeck()
    {
        return new List<CardData>();
    }

    public List<CardData> GetDeck()
    {
        return startingDeck;
    }

    public void SetCardsToDraw(int amount)
    {
        cardsToDraw = amount;
    }

    public int GetCardsToDraw()
    {
        return cardsToDraw;
    }

    public int GetHandSize()
    {
        return handSize;
    }
}
