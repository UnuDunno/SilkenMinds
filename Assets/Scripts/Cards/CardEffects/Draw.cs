using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Draw")]
public class Draw : CardEffect
{
    public override void ApplyEffect(Player player, EnemyData enemy)
    {
        player.SetCardsToDraw(amount);
    }
}
