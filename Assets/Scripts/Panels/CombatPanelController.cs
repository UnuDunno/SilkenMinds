using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;


public class CombatPanelController : PanelController
{
    [Header("Prefabs")]
    [SerializeField] private GameObject cardPrefab;

    [Header("Areas")]
    [SerializeField] private GameObject playerHandArea;
    [SerializeField] private GameObject enemyArea;
    [SerializeField] private Image enemyIcon;
    [SerializeField] private GameObject enemyInfos;
    [SerializeField] private GameObject playerDefense;
    [SerializeField] private TMP_Text fearText;

    [Header("Enemies")] 
    [SerializeField] private List<EnemyData> normalEnemies;
    [SerializeField] private List<EnemyData> eliteEnemies;
    [SerializeField] private List<EnemyData> bossEnemies;

    [Header("Enemies Info")]
    [SerializeField] private TMP_Text enemyName;
    [SerializeField] private TMP_Text enemyDescription;
    [SerializeField] private TMP_Text enemyCuriosities;
    [SerializeField] private TMP_Text reference;

    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private TMP_Text enemyHealthText;
    [SerializeField] private Image enemyBarFillColor;

    private NodeType nodeType;
    private int enemyCurrentHealth;
    private int enemyMaxHealth;

    // Player
    private List<Card> playerHand;
    private List<CardData> discardPile;
    private List<CardData> combatDeck;

    public void Awake()
    {
        EnemyArea enemy = enemyArea.GetComponent<EnemyArea>();
        enemy.updateCombat.AddListener(UpdateCombat);
        enemy.onDiscard.AddListener(DiscardCard);
    }

    public override void ResetPanel(NodeType nodeType)
    {
        base.ResetPanel(nodeType);
        this.nodeType = nodeType;

        playerDefense.SetActive(true);
        playerDefense.GetComponentInChildren<TMP_Text>().text = $"{player.GetDefense()}";
        fearText.text = $"{player.GetCurrentFear()}/{player.GetMaxFear()}";

        SetEnemy();

        playerHand = new List<Card>();
        discardPile = new List<CardData>();
        combatDeck = new List<CardData>(player.GetDeck());
        ShuffleDeck();

        PlayerTurn();
    }

    private EnemyData ChooseEnemy()
    {
        return nodeType switch
        {
            NodeType.Combat => normalEnemies[Random.Range(0, normalEnemies.Count)],
            NodeType.Elite => eliteEnemies[Random.Range(0, eliteEnemies.Count)],
            NodeType.Boss => bossEnemies[Random.Range(0, bossEnemies.Count)],
            _ => throw new System.Exception($"Invalid NodeType: {nodeType}"),
        };
    }

    private void SetEnemy()
    {
        EnemyData enemy = ChooseEnemy();

        enemyIcon.sprite = enemy.GetImage();
        enemyName.text = $"{enemy.GetName()}\n(<i>{enemy.GetCientificName()}</i>)";
        enemyDescription.text = enemy.GetDescription();

        StringBuilder stringBuilder = new StringBuilder();
        foreach(string str in enemy.GetCuriosities())
        {
            stringBuilder.AppendLine($"\u2022 <indent=1em>{str}</indent>");
        }
        enemyCuriosities.text = stringBuilder.ToString();

        reference.text = enemy.GetReference();

        enemyMaxHealth = enemy.GetHealth();
        enemyCurrentHealth = enemyMaxHealth;
        enemyHealthBar.GetComponent<Slider>().maxValue = enemyCurrentHealth;
        enemyHealthText.text = $"{enemyCurrentHealth}/{enemyMaxHealth}";

        EnemyArea area = enemyArea.GetComponent<EnemyArea>();
        area.SetEnemyData(enemy);
        area.SetPlayer(player);
    }

    private void EmptyPlayerHand()
    {
        while(playerHand.Count > 0)
        {
            Card card = playerHand[0];

            DiscardCard(card);
        }
    }

    private void DrawCards(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            if (combatDeck.Count == 0) ReshuffleDiscardPile();

            CardData drawnCard = combatDeck[0];

            GameObject cardGO = Instantiate(cardPrefab, playerHandArea.transform);
            Card card = cardGO.GetComponent<Card>();
            card.SetCardData(drawnCard);
            card.SetContainer(cardGO);

            playerHand.Add(card);
            combatDeck.Remove(drawnCard);
        }
    }

    private void PlayerTurn()
    {
        DrawCards(player.GetHandSize() - playerHand.Count);
        player.ResetDefense();
        player.ResetFear();

        UpdateUI();
    }

    public void EndTurn()
    {
        EmptyPlayerHand();
        EnemyTurn();
    }

    private void EnemyTurn()
    {
        Debug.Log("Enemy Turn");

        PlayerTurn();
    }

    public void ShowEnemyInfo()
    {
        enemyInfos.SetActive(!enemyInfos.activeSelf);
    }

    public void UpdateHealthBar()
    {
        float fillAmount = enemyCurrentHealth / enemyMaxHealth;
        if(fillAmount > 0.5f)
        {
            enemyBarFillColor.color = Color.Lerp(Color.yellow, Color.green, (fillAmount - 0.5f) * 2f);
        } else
        {
            enemyBarFillColor.color = Color.Lerp(Color.red, Color.yellow, fillAmount * 2f);
        }

        enemyHealthBar.GetComponent<Slider>().value = enemyCurrentHealth;
    }

    private void ShuffleDeck()
    {
        CardData tempCard;
        int randomIndex;
        for (int i = 0; i < combatDeck.Count; i++)
        {
            tempCard = combatDeck[i];
            randomIndex = Random.Range(i, combatDeck.Count);

            combatDeck[i] = combatDeck[randomIndex];
            combatDeck[randomIndex] = tempCard;
        }
    }

    private void ReshuffleDiscardPile()
    {
        combatDeck.AddRange(discardPile);
        discardPile.Clear();

        ShuffleDeck();
    }

    public void DiscardCard(Card card)
    {
        playerHand.Remove(card);
        discardPile.Add(card.GetCardData());
        card.DestroyCard();
    }

    public void UpdateCombat()
    {
        enemyCurrentHealth = enemyArea.GetComponent<EnemyArea>().GetEnemyData().GetHealth();

        DrawCards(player.GetCardsToDraw());
        player.SetCardsToDraw(0);

        UpdateUI();
    }

    public void UpdateUI()
    {
        enemyHealthText.text = $"{enemyCurrentHealth}/{enemyMaxHealth}";
        playerDefense.GetComponentInChildren<TMP_Text>().text = $"{player.GetDefense()}";
        fearText.text = $"{player.GetCurrentFear()}/{player.GetMaxFear()}";
        UpdateHealthBar();
    }

    public override void ClosePanel()
    {
        playerDefense.SetActive(false);

        base.ClosePanel();
    }
}
