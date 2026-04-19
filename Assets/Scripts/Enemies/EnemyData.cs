using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private Sprite image;
    [SerializeField] private string spiderName = "Aranha";
    [SerializeField] private string cientificName;
    [SerializeField] private int health = 5;
    [SerializeField] [TextArea] private string description = "Aranha comum";
    [SerializeField] private string reference;
    [SerializeField] private List<string> curiosities = new List<string>();
    [SerializeField] private int reward;
    [SerializeField] private List<Effect> actions;

    private int defense = 0;
    private Effect nextAction;

    // ********** GETTERS **********
    public Sprite GetImage() {  return image; }

    public string GetName() { return spiderName; }

    public string GetCientificName() { return cientificName; }

    public int GetHealth() { return health; }

    public string GetDescription() { return description; }

    public string GetReference() { return reference; }

    public List<string> GetCuriosities() { return curiosities; }

    public int GetReward() { return reward; }

    public int GetDefense() { return defense; }

    // ********** SETTERS **********
    public void SetDefense(int defense)
    {
        this.defense = defense;
    }

    // ********** COMBAT **********
    public void TakeDamage(int damage) 
    {
        defense -= damage;

        if(defense < 0)
        {
            health += defense;
            defense = 0;
        }

        if (health < 0) health = 0;
    }

    public void Heal(int amount)
    {
        health += amount;
    }

    public void SetNextAction()
    {
        nextAction = actions[UnityEngine.Random.Range(0, actions.Count)];
    }

    public Effect GetNextAction()
    {
        return nextAction;
    }
}
