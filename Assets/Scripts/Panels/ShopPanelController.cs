using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopPanelController : PanelController
{
    [Header("Shop")]
    [SerializeField] private GameObject shopContainerPrefab;
    [SerializeField] private GameObject cardsArea;
    [SerializeField] private TMP_Text healPriceText;
    [SerializeField] private TMP_Text cardRemovePriceText;

    [Header("Player")]
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text playerMoneyText;

    private int maxShopCards = 5;
    private int healPrice = 250;
    private int cardRemovePrice = 250;

    private CardData[] commonCards;
    private CardData[] epicCards;
    private CardData[] legendaryCards;

    public override void ResetPanel(NodeType nodeType)
    {
        base.ResetPanel(nodeType);

        if(commonCards == null)
        {
            commonCards = Resources.LoadAll<CardData>($"{path_to_card_in_store}/Common");
            epicCards = Resources.LoadAll<CardData>($"{path_to_card_in_store}/Epic");
            legendaryCards = Resources.LoadAll<CardData>($"{path_to_card_in_store}/Lendaria");
        }

        for(int i = 0; i < maxShopCards; i++)
        {
            GameObject cardContainer = Instantiate(shopContainerPrefab, cardsArea.transform);
            ShopContainer shopContainer = cardContainer.GetComponent<ShopContainer>();
            shopContainer.PlaceCardOnShop(ChooseShopCard());
            shopContainer.AddListenerBuyButton(BuyCard);
        }

        healPriceText.text = $"$ {healPrice}";
        cardRemovePriceText.text = $"$ {cardRemovePrice}";
    }

    public void RemoveCardFromDeck()
    {
        if (!(player.GetMoney() >= cardRemovePrice)) return;

        Debug.Log("Card Removed");

        playerMoneyText.text = $"{player.GetMoney()}";
    }

    public void RestoreHealth()
    {
        if (!(player.GetMoney() >= healPrice)) return;

        player.Heal(player.GetMaxHealth());

        playerHealthText.text = $"{player.GetCurrentHealth()}";
        playerMoneyText.text = $"{player.GetMoney()}";
    }

    public void BuyCard(CardData cardData)
    {
        if (!(player.GetMoney() >= cardData.cardPrice)) return;

        player.AddCardToDeck(cardData);
        player.DecreaseMoney(cardData.cardPrice);

        playerMoneyText.text = $"{player.GetMoney()}";
    }

    public CardData ChooseShopCard()
    {
        float randomNumber = UnityEngine.Random.value;

        if (randomNumber > rarityRates[Rarities.Legendary]) return legendaryCards[UnityEngine.Random.Range(0, legendaryCards.Length)];
        if (randomNumber > rarityRates[Rarities.Epic]) return epicCards[UnityEngine.Random.Range(0, epicCards.Length)];

        return commonCards[UnityEngine.Random.Range(0, commonCards.Length)];
    }

    public override void ClosePanel()
    {
        foreach (Transform child in cardsArea.transform)
        {
            Destroy(child.gameObject);
        }

        base.ClosePanel();
    }
}
