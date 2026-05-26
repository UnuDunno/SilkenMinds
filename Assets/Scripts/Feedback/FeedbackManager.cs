using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

[System.Serializable]
public class Feedback
{
    public List<int> answers;
    public string openAnswer;
}

public class FeedbackManager : MonoBehaviour
{
    public GameObject questionPrefab;
    public Transform feedbackContainer;
    public TMP_InputField openAnswer;

    private readonly List<string> questions = new List<string> {
        "As dicas sobre \"como agir em situações reais\" apresentados nos eventos foram claras e fáceis de entender.",
        "Os eventos e informações das aranhas ajudaram a identificar quais aranhas são perigosas e quais são inofensivas.",
        "Aprendi algo novo sobre a biologia das aranhas que não sabia antes de jogar.",
        "Após jogar, me sinto mais confiante em como agir quando encontrar uma aranha em casa.",
        "O nível de dificuldade das batalhas me motivou a querer jogar novamente para melhorar minha estratégia.",
        "A variedade de efeitos das cartas (Ataque, Defesa, Utilidade) mantiveram o jogo estimulante.",
        "O jogo ajudou a reduzir sentimentos negativos (como medo excessivo ou repulsa) em relação às aranhas.",
        "O jogo despertou curiosidade em vez de medo ou repulsa ao enfrentar uma nova espécie.",
        "Recomendaria esse jogo para alguém que tem interesse em aprender sobre aranhas ou que deseja perder o medo de aranhas",
    };

    private List<FeedbackQuestion> feedbackQuestions = new List<FeedbackQuestion>();
    private Feedback feedback;

    private void Start()
    {
        GenerateForm();
        feedback = new Feedback();
    }

    private void GenerateForm()
    {
        foreach(string question in questions)
        {
            GameObject item = Instantiate(questionPrefab, feedbackContainer);

            FeedbackQuestion feedbackQuestion = item.GetComponent<FeedbackQuestion>();
            feedbackQuestion.SetQuestionText(question);

            feedbackQuestions.Add(feedbackQuestion);
        }
    }

    public void SaveFromAnswers()
    {
        List<int> results = new List<int>();

        for (int i = 0; i < feedbackQuestions.Count; i++)
        {
            string active = feedbackQuestions[i].GetSelectedToggle();

            if (active == null)
            {
                Debug.Log($"Pergunta {i + 1}: VAZIO");
                continue;
            }

            int score = int.Parse(active);
            results.Add(score);
        }

        feedback.answers = results;
    }

    public void SendFeedback()
    {
        feedback.openAnswer = openAnswer.text;

        string json = JsonUtility.ToJson(feedback);
        string timeString = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");

        System.IO.File.WriteAllText(Application.persistentDataPath + $"/FeedbackAnswers-{timeString}.json", json);
    }
}
