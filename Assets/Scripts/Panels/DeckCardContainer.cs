using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeckCardContainer : MonoBehaviour
{
    [SerializeField] private Button removeCardButton;
    [SerializeField] private GameObject cardArea;

    [Header("Prefabs")]
    [SerializeField] private GameObject cardPrefab;

    private Card card;

    public void AddCardToContainer(CardData cardData)
    {
        if (cardData == null) return;

        GameObject cardGO = Instantiate(cardPrefab, cardArea.transform);
        Card newCard = cardGO.GetComponent<Card>();

        newCard.SetCardData(cardData);
        newCard.SetContainer(cardGO);
        newCard.NotDraggable();

        card = newCard;
    }

    public void ActivateRemoveButton()
    {
        removeCardButton.gameObject.SetActive(true);
    }

    public void DeactivateRemoveButton()
    {
        removeCardButton.gameObject.SetActive(false);
    }

    public void AddListenerRemoveButton(UnityAction<DeckCardContainer> action)
    {
        removeCardButton.onClick.RemoveAllListeners();
        removeCardButton.onClick.AddListener(() => action(this));
    }

    public CardData GetCardData()
    {
        return card.GetCardData();
    }

    public void RemoveCardContainer()
    {
        card.DestroyCard();
        Destroy(this.gameObject);
    }
}
