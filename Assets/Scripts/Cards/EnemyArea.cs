using UnityEngine;
using UnityEngine.Events;

public class EnemyArea : MonoBehaviour, ICardDropArea
{
    [SerializeField] private EnemyData enemy;
    [SerializeField] private Player player;

    public UnityEvent updateCombat;
    public UnityEvent<Card> onDiscard;

    public void SetEnemyData(EnemyData enemyData)
    {
        enemy = enemyData;
    }

    public EnemyData GetEnemyData()
    {
        return enemy;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public bool OnCardDrop(Card card)
    {
        CardData cardData = card.GetCardData();

        if (cardData.fearValue > player.GetCurrentFear()) return false;

        player.DecreaseCurrentFear(cardData.fearValue);

        foreach (Effect effect in cardData.effects)
        {
            effect.cardEffect.SetAmount(effect.value);
            effect.cardEffect.ApplyEffect(player, enemy, Target.Player);
        }

        onDiscard.Invoke(card);
        updateCombat.Invoke();

        return true;
    }
}
