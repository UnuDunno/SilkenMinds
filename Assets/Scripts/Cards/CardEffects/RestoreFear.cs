using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Restore Fear")]
public class RestoreFear : CardEffect
{
    public override void ApplyEffect(Player player, EnemyData enemy)
    {
        player.IncreaseCurrentFear(amount);
    }
}
