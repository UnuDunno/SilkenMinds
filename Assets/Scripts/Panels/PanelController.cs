using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Rarities
{
    Legendary,
    Epic,
    Common
}

public class PanelController : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] protected PlayerInput playerInput;
    [SerializeField] protected Player player;

    private static readonly string path_to_scriptable_objects = "ScriptableObjects";
    protected readonly string path_to_enemies = $"{path_to_scriptable_objects}/Enemies";

    private static readonly string path_to_cards = $"{path_to_scriptable_objects}/Cards";
    protected readonly string path_to_card_rewards = $"{path_to_cards}/Rewards";
    protected readonly string path_to_card_in_store = $"{path_to_cards}/Shop";

    protected readonly Dictionary<Rarities, float> rarityRates = new Dictionary<Rarities, float> {
        {Rarities.Legendary, 0.99f},
        {Rarities.Epic, 0.75f}
    };


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
