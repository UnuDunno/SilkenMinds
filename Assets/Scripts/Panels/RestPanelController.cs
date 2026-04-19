using UnityEngine;
using System;

public class RestPanelController : PanelController
{
    public void RemoveCardFromDeck()
    {
        Debug.Log("Card Removed");

        ClosePanel();
    }

    public void RestoreHealth()
    {
        int healAmount = (int)Math.Ceiling(player.GetMaxHealth() * player.GetHealthRegeneration());

        player.Heal(healAmount);

        ClosePanel();
    }
}
