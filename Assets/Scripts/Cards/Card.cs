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
    [SerializeField] private GameObject container;
    
    // ---  Drag  ---
    private Vector3 origin;
    private bool draggable = true;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!draggable) return;

        origin = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!draggable) return;

        transform.position += (Vector3)eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!draggable) return;

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

    public void SetContainer(GameObject container)
    {
        this.container = container;
    }

    public void NotDraggable()
    {
        draggable = false;
    }


    public void DestroyCard()
    {
        Destroy(container);
    }
}
