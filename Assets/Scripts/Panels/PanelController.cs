using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] private GameObject Panel;

    public void ClosePanel()
    {
        if (Panel == null) return;

        Panel.SetActive(false);
    }
}
