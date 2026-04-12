using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Block")]
public class Block : CardEffect
{
    public override void ApplyEffect(Player player, EnemyData enemy)
    {
        player.IncreaseDefense(amount);
    }
}
