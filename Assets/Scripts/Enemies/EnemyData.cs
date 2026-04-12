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

    private int defense = 0;

    // ********** GETTERS **********
    public Sprite GetImage() {  return image; }

    public string GetName() { return spiderName; }

    public string GetCientificName() { return cientificName; }

    public int GetHealth() { return health; }

    public string GetDescription() { return description; }

    public string GetReference() { return reference; }

    public List<string> GetCuriosities() { return curiosities; }

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

}
