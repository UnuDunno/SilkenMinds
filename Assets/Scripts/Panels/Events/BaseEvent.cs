using UnityEngine;

public enum Outcome
{
    // Positives
    IncreaseMoney,
    IncreaseMaxHealth,
    IncreaseMaxFear,
    //Negatives
    Damage,
    LoseCard
}

public abstract class BaseEvent : ScriptableObject
{
    public string eventTitle;
    [TextArea] public string eventDescription;
}
