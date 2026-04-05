using UnityEngine;

public class RestPanelController : PanelController
{
    public void RemoveCardFromDeck()
    {
        Debug.Log("Card Removed");

        ClosePanel();
    }

    public void RestoreHealth()
    {
        Debug.Log("Health Restored!");

        ClosePanel();
    }
}
