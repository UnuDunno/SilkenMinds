using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private Sprite image;
    [SerializeField] private string spiderName = "Aranha";
    [SerializeField] private string cientificName;
    [SerializeField] private float health = 5f;
    [SerializeField] [TextArea] private string description = "Aranha comum";
    [SerializeField] private string reference;
    [SerializeField] private List<string> curiosities = new List<string>();
}
