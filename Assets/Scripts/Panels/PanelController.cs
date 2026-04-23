using System;
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

    private static readonly string path_to_events = $"{path_to_scriptable_objects}/Events";
    protected readonly string path_to_curiosity_events = $"{path_to_events}/Curiosity";
    protected readonly string path_to_choice_events = $"{path_to_events}/Choice";

    protected readonly Dictionary<Rarities, float> rarityRates = new Dictionary<Rarities, float> {
        {Rarities.Legendary, 0.99f},
        {Rarities.Epic, 0.75f}
    };

    public virtual void ResetPanel(NodeType nodeType)
    {
        if (player.EmptyInput()) player.AddInput("Map");
        else player.AddInput("Panel");
        
        playerInput.SwitchCurrentActionMap("Panel");
    }

    public virtual void ClosePanel()
    {
        if (panel == null) return;

        playerInput.SwitchCurrentActionMap(player.RemoveInput());
        panel.SetActive(false);
    }
}
