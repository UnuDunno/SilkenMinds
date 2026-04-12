using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Heal")]
public class Heal : CardEffect
{
    public override void ApplyEffect(Player player, EnemyData enemy)
    {
        player.Heal(amount);
    }
}
