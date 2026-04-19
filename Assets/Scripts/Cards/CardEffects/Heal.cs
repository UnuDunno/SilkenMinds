using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Heal")]
public class Heal : CardEffect
{
    public override void ApplyEffect(Player player, EnemyData enemy, Target target)
    {
        switch(target)
        {
            case Target.Player:
                player.Heal(amount);
                break;
            case Target.Enemy:
                enemy.Heal(amount);
                break;
        }
    }
}
