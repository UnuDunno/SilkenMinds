using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChoicePanelController : MonoBehaviour
{
    [SerializeField] private TMP_Text eventTitle;
    [SerializeField] private TMP_Text eventDescription;

    [Header("Choice 1")]
    [SerializeField] private Button choice1Button;
    [SerializeField] private TMP_Text choice1ButtonText;

    [Header("Choice 2")]
    [SerializeField] private Button choice2Button;
    [SerializeField] private TMP_Text choice2ButtonText;

    [Header("Outcome")]
    [SerializeField] private GameObject result;
    [SerializeField] private TMP_Text outcomeTitle;
    [SerializeField] private TMP_Text outcomeText;
    [SerializeField] private Button endEventButton;
    [SerializeField] private TMP_Text endEventButtonText;

    public void SetPanel(ChoiceEvent curiosityEvent, UnityAction closeMainPanel, UnityAction<Outcome, int> applyOutcome)
    {
        eventTitle.text = curiosityEvent.eventTitle;
        eventDescription.text = curiosityEvent.eventDescription;

        choice1ButtonText.text = curiosityEvent.choice1;
        choice2ButtonText.text = curiosityEvent.choice2;

        choice1Button.onClick.RemoveAllListeners();
        choice1Button.onClick.AddListener(
            () => ChoiceOutcome(
                curiosityEvent.outcome1, 
                curiosityEvent.amount1, 
                curiosityEvent.outcome1Title, 
                curiosityEvent.outcome1Text, 
                applyOutcome,
                closeMainPanel
            )
        );

        choice2Button.onClick.RemoveAllListeners();
        choice2Button.onClick.AddListener(
            () => ChoiceOutcome(
                curiosityEvent.outcome2,
                curiosityEvent.amount2,
                curiosityEvent.outecome2Title,
                curiosityEvent.outcome2Text,
                applyOutcome,
                closeMainPanel
            )
        );

        gameObject.SetActive(true);
    }

    private void ChoiceOutcome(Outcome outcome, int outcomeAmount, string title, string text, UnityAction<Outcome, int> applyOutcome, UnityAction closeMainPanel)
    {
        outcomeTitle.text = title;
        outcomeText.text = text;

        endEventButtonText.text = GetTextoOutcome(outcome, outcomeAmount);

        endEventButton.onClick.RemoveAllListeners();
        endEventButton.onClick.AddListener(() => applyOutcome(outcome, outcomeAmount));
        endEventButton.onClick.AddListener(ClosePanel);
        endEventButton.onClick.AddListener(closeMainPanel);

        result.SetActive(true);
    }

    private string GetTextoOutcome(Outcome outcome, int amount)
    {
        return outcome switch
        {
            Outcome.IncreaseMaxHealth => $"Aumentar Vida ({amount})",
            Outcome.IncreaseMaxFear => $"Aumentar o Medo ({amount})",
            Outcome.IncreaseMoney => $"Receber $ {amount}",
            Outcome.Damage => $"Receber {amount} dano",
            Outcome.LoseCard => $"Perder {amount} carta(s) aleatória(s)",
            _ => "",
        };
    }

    private void ClosePanel()
    {
        result.SetActive(false);
        gameObject.SetActive(false);
    }
}
