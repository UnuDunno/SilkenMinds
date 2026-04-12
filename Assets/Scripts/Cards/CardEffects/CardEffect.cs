using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    protected int amount;

    public void SetAmount(int amount)
    {
        this.amount = amount;
    }

    public abstract void ApplyEffect(Player player, EnemyData enemy);
}
