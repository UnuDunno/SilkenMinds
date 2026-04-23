using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CuriosityPanelController : MonoBehaviour
{
    [SerializeField] private TMP_Text eventTitle;
    [SerializeField] private TMP_Text eventDescription;
    [SerializeField] private Button completeEventButton;
    [SerializeField] private TMP_Text completeEventButtonText;

    private CuriosityEvent currentEvent;

    public void SetPanel(CuriosityEvent curiosityEvent, UnityAction closeMainPanel, UnityAction<Outcome, int> applyOutcome)
    {
        currentEvent = curiosityEvent;

        eventTitle.text = curiosityEvent.eventTitle;
        eventDescription.text = curiosityEvent.eventDescription;

        Outcome outcome = curiosityEvent.outcome;
        int outcomeAmount = curiosityEvent.outcomeAmount;

        completeEventButtonText.text = GetTextoRecompensa(outcome, outcomeAmount);

        completeEventButton.onClick.RemoveAllListeners();
        completeEventButton.onClick.AddListener(ClosePanel);
        completeEventButton.onClick.AddListener(closeMainPanel);
        completeEventButton.onClick.AddListener(() => applyOutcome(outcome, outcomeAmount));

        gameObject.SetActive(true);
    }

    private string GetTextoRecompensa(Outcome outcome, int amount)
    {
        return outcome switch
        {
            Outcome.IncreaseMaxHealth => $"Aumentar Vida ({amount})",
            Outcome.IncreaseMaxFear => $"Aumentar o Medo ({amount})",
            Outcome.IncreaseMoney => $"Receber $ {amount}",
            Outcome.Damage => "",
            Outcome.LoseCard => "",
            _ => "",
        };
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
