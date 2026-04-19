using UnityEngine;

public enum Target
{
    Player,
    Enemy
}

public abstract class CardEffect : ScriptableObject
{
    protected int amount;

    public void SetAmount(int amount)
    {
        this.amount = amount;
    }

    public abstract void ApplyEffect(Player player, EnemyData enemy, Target target);
}
