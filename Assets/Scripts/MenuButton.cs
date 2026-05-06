using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Game Closed");
    }
}
