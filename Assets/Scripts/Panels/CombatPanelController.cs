using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CombatPanelController : PanelController
{
    [Header("Prefabs")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject rewardContainerPrefab;
    [SerializeField] private GameObject cardsArea;

    [Header("Areas")]
    [SerializeField] private GameObject playerHandArea;
    [SerializeField] private GameObject enemyArea;
    [SerializeField] private Image enemyIcon;
    [SerializeField] private GameObject enemyInfos;
    [SerializeField] private GameObject playerDefense;
    [SerializeField] private TMP_Text fearText;

    // ENEMIES
    private EnemyData[] normalEnemies;
    private EnemyData[] eliteEnemies;
    private EnemyData[] bossEnemies;

    [Header("Enemies Info")]
    [SerializeField] private TMP_Text enemyName;
    [SerializeField] private TMP_Text enemyDescription;
    [SerializeField] private TMP_Text enemyCuriosities;
    [SerializeField] private TMP_Text reference;

    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private TMP_Text enemyHealthText;
    [SerializeField] private Image enemyBarFillColor;

    [SerializeField] private Image enemyIntentIcon;
    [SerializeField] private TMP_Text enemyIntentText;

    [SerializeField] private GameObject enemyDefenseArea;
    [SerializeField] private TMP_Text enemyDefenseText;

    [Header("End Combat")]
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private TMP_Text rewardText;
    private int maxRewardCards = 3;

    // CARDS
    private CardData[] commonRewards;
    private CardData[] epicRewards;
    private CardData[] legendaryRewards;

    private NodeType nodeType;
    private int enemyCurrentHealth;
    private int enemyMaxHealth;

    [Header("Player")]
    [SerializeField] private TMP_Text playerHealth;
    [SerializeField] private TMP_Text playerMoney;
    private List<Card> playerHand;
    private List<CardData> discardPile;
    private List<CardData> combatDeck;

    private readonly int noCardMoneyRewardMultiplier = 2;

    public override void ResetPanel(NodeType nodeType)
    {
        base.ResetPanel(nodeType);
        this.nodeType = nodeType;

        playerDefense.SetActive(true);
        playerDefense.GetComponentInChildren<TMP_Text>().text = $"{player.GetDefense()}";
        fearText.text = $"{player.GetCurrentFear()}/{player.GetMaxFear()}";

        if(normalEnemies == null)
        {
            EnemyArea enemy = enemyArea.GetComponent<EnemyArea>();
            enemy.updateCombat.AddListener(UpdateCombat);
            enemy.onDiscard.AddListener(DiscardCard);

            normalEnemies = Resources.LoadAll<EnemyData>($"{path_to_enemies}/Normal");
            eliteEnemies = Resources.LoadAll<EnemyData>($"{path_to_enemies}/Elite");
            bossEnemies = Resources.LoadAll<EnemyData>($"{path_to_enemies}/Boss");

            commonRewards = Resources.LoadAll<CardData>($"{path_to_card_rewards}/Common");
            epicRewards = Resources.LoadAll<CardData>($"{path_to_card_rewards}/Epic");
            legendaryRewards = Resources.LoadAll<CardData>($"{path_to_card_rewards}/Lendaria");
        }

        SetEnemy();
        UpdateEnemyIntent();

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
            NodeType.Combat => normalEnemies[UnityEngine.Random.Range(0, normalEnemies.Length)],
            NodeType.Elite => eliteEnemies[UnityEngine.Random.Range(0, eliteEnemies.Length)],
            NodeType.Boss => bossEnemies[UnityEngine.Random.Range(0, bossEnemies.Length)],
            _ => throw new System.Exception($"Invalid NodeType: {nodeType}"),
        };
    }

    private void SetEnemy()
    {
        EnemyData combatEnemy = UnityEngine.Object.Instantiate(ChooseEnemy());

        enemyIcon.sprite = combatEnemy.GetImage();
        enemyName.text = $"{combatEnemy.GetName()}\n(<i>{combatEnemy.GetCientificName()}</i>)";
        enemyDescription.text = combatEnemy.GetDescription();

        StringBuilder stringBuilder = new StringBuilder();
        foreach(string str in combatEnemy.GetCuriosities())
        {
            stringBuilder.AppendLine($"\u2022 <indent=1em>{str}</indent>");
        }
        enemyCuriosities.text = stringBuilder.ToString();

        reference.text = combatEnemy.GetReference();

        enemyMaxHealth = combatEnemy.GetHealth();
        enemyCurrentHealth = enemyMaxHealth;
        enemyHealthBar.GetComponent<Slider>().maxValue = enemyCurrentHealth;
        enemyHealthText.text = $"{enemyCurrentHealth}/{enemyMaxHealth}";

        combatEnemy.SetNextAction();
        combatEnemy.SetDefense(0);

        EnemyArea area = enemyArea.GetComponent<EnemyArea>();
        area.SetEnemyData(combatEnemy);
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
        EnemyData enemy = enemyArea.GetComponent<EnemyArea>().GetEnemyData();
        enemy.SetDefense(0);

        Effect enemyAction = enemy.GetNextAction();

        enemyAction.cardEffect.SetAmount(enemyAction.value);
        enemyAction.cardEffect.ApplyEffect(player, enemy, Target.Enemy);

        enemy.SetNextAction();

        UpdateCombat();
        UpdateEnemyIntent();

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
            randomIndex = UnityEngine.Random.Range(i, combatDeck.Count);

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

        if(player.GetCurrentHealth() <= 0)
        {
            Defeat();
        } else if(enemyCurrentHealth <= 0)
        {
            Victory();
        }
    }

    public void Victory()
    {
        if (nodeType == NodeType.Boss) SceneManager.LoadScene("VictoryScene");

        int moneyReward = enemyArea.GetComponent<EnemyArea>().GetEnemyData().GetReward();
        rewardText.text = $"Ignorar Cartas\n(Receber <color=#EFBF04>$ {moneyReward * noCardMoneyRewardMultiplier}</color>)";

        for (int i = 0; i < maxRewardCards; i++)
        {
            GameObject cardContainer = Instantiate(rewardContainerPrefab, cardsArea.transform);
            ShopContainer shopContainer = cardContainer.GetComponent<ShopContainer>();
            shopContainer.PlaceCardOnShop(ChooseCardReward());
            shopContainer.AddListenerBuyButton(AddCardToDeck);
            shopContainer.DeactivateCoinIcon();
            shopContainer.ChangeCardPrice("OBTER", color: "FFFFFF");
        }

        EmptyPlayerHand();

        player.IncreaseMoney(moneyReward);
        playerMoney.text = $"{player.GetMoney()}";

        victoryScreen.SetActive(true);
    }

    public void Defeat()
    {
        SceneManager.LoadScene("DefeatScene");
    }

    public void AddCardToDeck(CardData cardData)
    {
        player.AddCardToDeck(cardData);

        ClosePanel();
    }

    public void GetMoneyReward()
    {
        player.IncreaseMoney(enemyArea.GetComponent<EnemyArea>().GetEnemyData().GetReward() * noCardMoneyRewardMultiplier);

        playerMoney.text = $"{player.GetMoney()}";

        ClosePanel();
    }

    public CardData ChooseCardReward()
    {
        float randomValue = UnityEngine.Random.value;

        if (randomValue > rarityRates[Rarities.Legendary])
        {
            return legendaryRewards[UnityEngine.Random.Range(0, legendaryRewards.Length)];
        } else if (randomValue > rarityRates[Rarities.Epic])
        {
            return epicRewards[UnityEngine.Random.Range(0, epicRewards.Length)];
        }

        return commonRewards[UnityEngine.Random.Range(0, commonRewards.Length)];
    }

    public void UpdateUI()
    {
        // Enemy
        enemyHealthText.text = $"{enemyCurrentHealth}/{enemyMaxHealth}";
        playerDefense.GetComponentInChildren<TMP_Text>().text = $"{player.GetDefense()}";
        fearText.text = $"{player.GetCurrentFear()}/{player.GetMaxFear()}";
        UpdateHealthBar();

        int enemyDefense = enemyArea.GetComponent<EnemyArea>().GetEnemyData().GetDefense();
        if(enemyDefense > 0)
        {
            enemyDefenseArea.SetActive(true);

            enemyDefenseText.text = $"{enemyDefense}";
        }
        else
        {
            enemyDefenseArea.SetActive(false);
        }

        // Player
        playerHealth.text = $"{player.GetCurrentHealth()}/{player.GetMaxHealth()}";
    }

    private void UpdateEnemyIntent()
    {
        EnemyData enemy = enemyArea.GetComponent<EnemyArea>().GetEnemyData();
        Effect intent = enemy.GetNextAction();

        enemyIntentIcon.sprite = intent.effectImage;
        enemyIntentText.text = $"{intent.value}";
    }

    public override void ClosePanel()
    {
        playerDefense.SetActive(false);
        victoryScreen.SetActive(false);

        foreach (Transform child in cardsArea.transform)
        {
            Destroy(child.gameObject);
        }

        base.ClosePanel();
    }
}
