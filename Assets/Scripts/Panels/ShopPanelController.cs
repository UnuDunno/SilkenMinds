using System.Collections.Generic;
using UnityEngine;

public class ShopPanelController : PanelController
{
    [SerializeField] private GameObject shopContainerPrefab;
    [SerializeField] private GameObject cardsArea;

    private List<GameObject> cardsInShop;

    private int maxShopCards = 5;

    public override void ResetPanel(NodeType nodeType)
    {
        base.ResetPanel(nodeType);

        cardsInShop = new List<GameObject>();

        for(int i = 0; i < maxShopCards; i++)
        {
            GameObject cardContainer = Instantiate(shopContainerPrefab, cardsArea.transform);
            ShopContainer shopContainer = cardContainer.GetComponent<ShopContainer>();
            shopContainer.PlaceCardOnShop(ChooseShopCard());
            shopContainer.AddListenerBuyButton(BuyCard);

            cardsInShop.Add(cardContainer);
        }
    }

    public void RemoveCardFromDeck()
    {
        Debug.Log("Card Removed");
    }

    public void RestoreHealth()
    {
        Debug.Log("Health Restored");
    }

    public void BuyCard(CardData cardData)
    {
        Debug.Log("Card Bought");
    }

    public CardData ChooseShopCard()
    {
        return new CardData();
    }

    public override void ClosePanel()
    {
        foreach(GameObject card in cardsInShop)
        {
            Destroy(card);
        }

        base.ClosePanel();
    }
}
