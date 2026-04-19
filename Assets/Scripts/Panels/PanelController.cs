using UnityEngine;
using UnityEngine.InputSystem;

public class PanelController : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] protected PlayerInput playerInput;
    [SerializeField] protected Player player;

    private static readonly string path_to_scriptable_objects = "ScriptableObjects";
    protected readonly string path_to_enemies = $"{path_to_scriptable_objects}/Enemies";
    protected readonly string path_to_card_rewards = $"{path_to_scriptable_objects}/Cards/Rewards";

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
