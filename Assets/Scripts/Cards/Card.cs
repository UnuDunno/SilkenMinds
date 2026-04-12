using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Collider2D cardCollider;
    [SerializeField] private CardData cardData;
    [SerializeField] private Image cardIcon;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private TMP_Text cardFear;
    
    // ---  Drag  ---
    private Vector3 origin;

    public void OnBeginDrag(PointerEventData eventData)
    {
        origin = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cardCollider.enabled = false;
        Collider2D hitCollider = Physics2D.OverlapPoint(transform.position);
        cardCollider.enabled = true;

        bool resetPosition = true;
        if (hitCollider != null && hitCollider.TryGetComponent(out ICardDropArea cardDropArea))
        {
            resetPosition = !cardDropArea.OnCardDrop(this);
        }
        
        if(resetPosition)
        {
            transform.position = origin;
        }
    }

    public void SetCardData(CardData data)
    {
        cardData = data;

        cardIcon.sprite = cardData.cardImage;
        cardName.text = cardData.cardName;
        cardDescription.text = cardData.cardDescription;
        cardFear.text = $"{cardData.fearValue}";
    }

    public CardData GetCardData()
    {
        return cardData;
    }

    public virtual void ApplyEffect(EnemyData enemy, Player player)
    {
        player.DecreaseCurrentFear(cardData.fearValue);
    }

}
