using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ShopContainer : MonoBehaviour
{
    [SerializeField] private GameObject cardContainer;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Button buyCardButton;
    [SerializeField] private TMP_Text cardPrice;
    [SerializeField] private GameObject coinImage;

    private CardData cardData;

    public void PlaceCardOnShop(CardData cardData)
    {
        if (cardData == null) return;

        GameObject cardGO = Instantiate(cardPrefab, cardContainer.transform);
        Card card = cardGO.GetComponent<Card>();
        card.SetCardData(cardData);
        card.SetContainer(cardGO);
        card.NotDraggable();

        cardPrice.text = $"$ {cardData.cardPrice}";

        this.cardData = cardData;
    }

    public void AddListenerBuyButton(UnityAction<CardData, Button> action)
    {
        buyCardButton.onClick.RemoveAllListeners();
        buyCardButton.onClick.AddListener(() => action(cardData, buyCardButton));
    }

    public void DeactivateCoinIcon()
    {
        coinImage.SetActive(false);
    }

    public void ChangeCardPrice(string valor, string color = "EFBF04")
    {
        cardPrice.text = $"<color=#{color}>{valor}</color>";
    }
}
