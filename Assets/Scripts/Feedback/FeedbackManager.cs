using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackManager : MonoBehaviour
{
    public List<FeedbackQuestion> questions;
    public GameObject questionPrefab;
    public Transform feedbackContainer;

    private List<ToggleGroup> answerGroup = new List<ToggleGroup>();

    private void Start()
    {
        GenerateForm();
    }

    private void GenerateForm()
    {
        foreach(FeedbackQuestion question in questions)
        {
            GameObject item = Instantiate(questionPrefab, feedbackContainer);
            item.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = question.questionText;

            answerGroup.Add(item.GetComponentInChildren<ToggleGroup>());
        }
    }

    public void SendFeedback()
    {
        List<int> results = new List<int>();

        for (int i = 0; i < answerGroup.Count; i++)
        {
            Toggle active = answerGroup[i].ActiveToggles().FirstOrDefault();

            if (active == null) continue;

            int score = int.Parse(active.name);
            results.Add(score);

            Debug.Log($"Pergunta {i + 1}: Resposta {score}");
        }
    }
}
