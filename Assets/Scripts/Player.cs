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

    [SerializeField] private float healthRegeneration = 0.3f;

    [SerializeField] private DeckPanelController deckPanel;

    private int currentHealth;
    private int currentFear;
    private int currentDefense;
    private int cardsToDraw;
    private int fearToRemove;

    private static readonly string path_to_scriptable_objects = "ScriptableObjects";
    private readonly string path_to_main_deck_cards = $"{path_to_scriptable_objects}/Cards/MainDeck";

    private Stack<string> inputSequence = new Stack<string>();

    private void Start()
    {
        maxHealth = 30;
        handSize = 5;
        money = 500;
        maxFear = 5;

        CreateStartingDeck();

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

    public float GetHealthRegeneration()
    {
        return healthRegeneration;
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

    public void SetFearToRemove(int amount)
    {
        fearToRemove = amount;
    }

    public void ResetFear()
    {
        currentFear = maxFear - fearToRemove;
        currentFear = Mathf.Max(currentFear, 0);

        fearToRemove = 0;
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
    private void CreateStartingDeck()
    {
        startingDeck = new List<CardData>();
        CardData[] mainDeckCards = Resources.LoadAll<CardData>(path_to_main_deck_cards);

        foreach(CardData cardData in mainDeckCards)
        {
            switch(cardData.cardName.ToLower())
            {
                case "golpe":
                    for (int i = 0; i < 3; i++) AddCardToDeck(cardData);
                    break;
                case "bloqueio":
                    for(int i = 0; i < 3; i++) AddCardToDeck(cardData);
                    break;
                case "olhos de caçador":
                    for (int i = 0; i < 2; i++) AddCardToDeck(cardData);
                    break;
                case "foco aracnídeo":
                    for (int i = 0; i < 1; i++) AddCardToDeck(cardData);
                    break;
                case "teia da vida":
                    for (int i = 0; i < 1; i++) AddCardToDeck(cardData);
                    break;
                default:
                    Debug.Log($"Unexpected card in the starting deck: {cardData.cardName}");
                    break;
            }
        }
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

    public void AddCardToDeck(CardData card)
    {
        startingDeck.Add(card);
        deckPanel.AddCardToDeck(card);
    }

    public void RemoveCardFromDeck(CardData card)
    {
        startingDeck.Remove(card);
    }

    // ********** INPUT **********
    public void AddInput(string inputName)
    {
        inputSequence.Push(inputName);
    }

    public string RemoveInput()
    {
        return inputSequence.Pop();
    }

    public bool EmptyInput()
    {
        return inputSequence.Count == 0;
    }
}
