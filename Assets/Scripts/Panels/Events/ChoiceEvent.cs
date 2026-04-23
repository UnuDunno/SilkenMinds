using UnityEngine;

[CreateAssetMenu(fileName = "ChoiceEvent", menuName = "Event/ChoiceEvent")]
public class ChoiceEvent : BaseEvent
{
    [Header("Choice 1")]
    public string choice1;
    public Outcome outcome1;
    public int amount1;
    public string outcome1Title;
    [TextArea] public string outcome1Text;

    [Header("Choice 2")]
    public string choice2;
    public Outcome outcome2;
    public int amount2;
    public string outecome2Title;
    [TextArea] public string outcome2Text;
}
