using UnityEngine;

public class EnemyArea : MonoBehaviour, ICardDropArea
{
    public void OnCardDrop(Card card)
    {
        Destroy(card.gameObject);
    }
}
