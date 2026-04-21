using UnityEngine;
using System;

public class RestPanelController : PanelController
{
    public void IncreaseFear()
    {
        player.IncreaseMaxFear(1);

        ClosePanel();
    }

    public void RestoreHealth()
    {
        int healAmount = (int)Math.Ceiling(player.GetMaxHealth() * player.GetHealthRegeneration());

        player.Heal(healAmount);

        ClosePanel();
    }
}
