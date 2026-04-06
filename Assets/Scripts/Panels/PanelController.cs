using UnityEngine;
using UnityEngine.InputSystem;

public class PanelController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        playerInput.SwitchCurrentActionMap("Panel");
    }

    public void ClosePanel()
    {
        if (panel == null) return;

        playerInput.SwitchCurrentActionMap("Map");
        panel.SetActive(false);
    }
}
