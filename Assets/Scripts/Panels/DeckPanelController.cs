using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeckPanelController : PanelController
{
    [Header("Prefabs")]
    [SerializeField] private GameObject cardContainerPrefab;

    [Header("Areas")]
    [SerializeField] private GameObject cardsArea;
    [SerializeField] private GameObject confirmActionPanel;
    [SerializeField] private TMP_Text confirmText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button closeButtonFromShop;

    private List<DeckCardContainer> deckCards = new List<DeckCardContainer>();

    public void OpenPanel()
    {
        base.ResetPanel(NodeType.Event);
        gameObject.SetActive(true);
    }

    public void AddCardToDeck(CardData card)
    {
        GameObject cardContainerGO = Instantiate(cardContainerPrefab, cardsArea.transform);
        DeckCardContainer cardContainer = cardContainerGO.GetComponent<DeckCardContainer>();

        cardContainer.AddCardToContainer(card);

        deckCards.Add(cardContainer);
    }

    public void RemoveCardFromDeck(DeckCardContainer cardContainer)
    {
        if (deckCards.Count <= 0) return;

        player.RemoveCardFromDeck(cardContainer.GetCardData());

        deckCards.Remove(cardContainer);

        cardContainer.RemoveCardContainer();
    }

    public void RemoveRandomCardFromDeck()
    {
        if (deckCards.Count <= 0) return;

        DeckCardContainer cardContainer = deckCards[UnityEngine.Random.Range(0, deckCards.Count)];

        RemoveCardFromDeck(cardContainer);
    }

    public void ActivateAllRemoveButtons(UnityAction<DeckCardContainer> action)
    {
        foreach (DeckCardContainer cardContainer in deckCards)
        {
            cardContainer.AddListenerRemoveButton(action);
            cardContainer.ActivateRemoveButton();
        }
    }

    public void DeactivateAllRemoveButtons()
    {
        foreach(DeckCardContainer cardContainer in deckCards)
        {
            cardContainer.DeactivateRemoveButton();
        }
    }

    public void OpenPanelFromShop(UnityAction<DeckCardContainer> removeCard)
    {
        ActivateAllRemoveButtons(removeCard);
        closeButton.gameObject.SetActive(false);
        closeButtonFromShop.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void ColsePanelFromShop()
    {
        DeactivateAllRemoveButtons();
        closeButton.gameObject.SetActive(true);
        closeButtonFromShop.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
