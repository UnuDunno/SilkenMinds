using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeedbackQuestion : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private ToggleGroup answers;

    public void SetQuestionText(string text)
    {
        questionText.text = text;
    }

    public string GetQuestionText()
    {
        return questionText.text;
    }

    public string GetSelectedToggle()
    {
        // Get the first active toggle in the group
        //Toggle activeToggle = answers.ActiveToggles().FirstOrDefault();

        return answers.GetFirstActiveToggle().name;
    }
}
