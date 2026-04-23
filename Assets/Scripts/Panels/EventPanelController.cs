using UnityEngine;
using System;

public enum EventType
{
    Choice,
    Curiosity
}

public class EventPanelController : PanelController
{
    [Header("Curiosity Events")]
    [SerializeField] private CuriosityPanelController curiosityEventPanel;

    [Header("Choice Events")]
    [SerializeField] private ChoicePanelController choiceEventPanel;

    [Header("General")]
    [SerializeField] private DeckPanelController deckPanelController;

    private CuriosityEvent[] curiosityEvents;
    private ChoiceEvent[] choiceEvents;

    public override void ResetPanel(NodeType nodeType)
    {
        base.ResetPanel(nodeType);

        if(curiosityEvents == null)
        {
            curiosityEvents = Resources.LoadAll<CuriosityEvent>(path_to_curiosity_events);
            choiceEvents = Resources.LoadAll<ChoiceEvent>(path_to_choice_events);
        }

        EventType eventType = (EventType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EventType)).Length);

        switch (eventType)
        { 
            case EventType.Choice:
                choiceEventPanel.SetPanel(
                    choiceEvents[UnityEngine.Random.Range(0, choiceEvents.Length)], 
                    ClosePanel, 
                    ApplyOutcome
                );
                break;
            case EventType.Curiosity:
                curiosityEventPanel.SetPanel(
                    curiosityEvents[UnityEngine.Random.Range(0, curiosityEvents.Length)], 
                    ClosePanel, 
                    ApplyOutcome
                );
                break;
        }
    }

    public void ApplyOutcome(Outcome outcome, int amount)
    {
        switch (outcome)
        {
            case Outcome.IncreaseMoney:
                player.IncreaseMoney(amount);
                break;
            case Outcome.IncreaseMaxFear:
                player.IncreaseMaxFear(amount);
                break;
            case Outcome.IncreaseMaxHealth:
                player.IncreaseMaxHealth(amount);
                break;
            case Outcome.Damage:
                player.TakeDamage(amount);
                break;
            case Outcome.LoseCard:
                for (int i = 0; i < amount; i++) deckPanelController.RemoveRandomCardFromDeck();
                break;
        }
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
    }
}
