using UnityEngine;
using UnityEngine.InputSystem;

public class PanelController : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] protected PlayerInput playerInput;
    [SerializeField] protected Player player;

    public virtual void ResetPanel(NodeType nodeType)
    {
        playerInput.SwitchCurrentActionMap("Panel");
    }

    public virtual void ClosePanel()
    {
        if (panel == null) return;

        playerInput.SwitchCurrentActionMap("Map");
        panel.SetActive(false);
    }
}
