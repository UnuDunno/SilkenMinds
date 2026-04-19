using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Block")]
public class Block : CardEffect
{
    public override void ApplyEffect(Player player, EnemyData enemy, Target target)
    {
        switch(target)
        {
            case Target.Player:
                player.IncreaseDefense(amount);
                break;
            case Target.Enemy:
                enemy.SetDefense(amount);
                break;
        }
    }
}
