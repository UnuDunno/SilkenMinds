using UnityEngine;

[CreateAssetMenu(fileName = "CuriosityEvent", menuName = "Event/CuriosityEvent")]
public class CuriosityEvent : BaseEvent
{
    public Outcome outcome;
    public int outcomeAmount;
}
