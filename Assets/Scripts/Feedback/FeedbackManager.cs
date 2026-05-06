using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackManager : MonoBehaviour
{
    public GameObject questionPrefab;
    public Transform feedbackContainer;

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

    private List<ToggleGroup> answerGroup = new List<ToggleGroup>();

    private void Start()
    {
        GenerateForm();
    }

    private void GenerateForm()
    {
        foreach(string question in questions)
        {
            GameObject item = Instantiate(questionPrefab, feedbackContainer);
            item.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = question;

            answerGroup.Add(item.GetComponentInChildren<ToggleGroup>());
        }
    }

    public void SendFeedback()
    {
        List<int> results = new List<int>();

        for (int i = 0; i < answerGroup.Count; i++)
        {
            Toggle active = answerGroup[i].ActiveToggles().FirstOrDefault();

            if (active == null)
            {
                Debug.Log("VAZIO");
                continue;
            }

            int score = int.Parse(active.name);
            results.Add(score);

            Debug.Log($"Pergunta {i + 1}: Resposta {score}");
        }
    }
}
