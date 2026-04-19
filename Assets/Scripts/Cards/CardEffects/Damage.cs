using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Damage")]
public class Damage : CardEffect
{
    public override void ApplyEffect(Player player, EnemyData enemy, Target target)
    {
        switch(target)
        {
            case Target.Player:
                enemy.TakeDamage(amount);
                break;
            case Target.Enemy:
                player.TakeDamage(amount);
                break;
        }
    }
}
