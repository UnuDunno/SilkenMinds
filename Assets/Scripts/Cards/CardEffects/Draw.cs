using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Draw")]
public class Draw : CardEffect
{
    public override void ApplyEffect(Player player, EnemyData enemy, Target target)
    {
        switch(target)
        {
            case Target.Player:
                player.SetCardsToDraw(amount);
                break;
            case Target.Enemy:
                break;
        }
    }
}
