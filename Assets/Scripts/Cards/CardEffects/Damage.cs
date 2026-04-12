using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Damage")]
public class Damage : CardEffect
{
    public override void ApplyEffect(Player player, EnemyData enemy)
    {
        enemy.TakeDamage(amount);
    }
}
