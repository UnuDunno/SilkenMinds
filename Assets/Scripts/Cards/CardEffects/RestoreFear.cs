using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Restore Fear")]
public class RestoreFear : CardEffect
{
    public override void ApplyEffect(Player player, EnemyData enemy, Target target)
    {
        switch(target)
        {
            case Target.Player:
                player.IncreaseCurrentFear(amount);
                break;
            case Target.Enemy:
                player.SetFearToRemove(amount);
                break;

        }
    }
}
