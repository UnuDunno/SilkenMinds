using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private Sprite Image;
    [SerializeField] private string Name = "Aranha";
    [SerializeField] private string CientificName;
    [SerializeField] private float Health = 5f;
    [SerializeField] [TextArea] private string Description = "Aranha comum";
    [SerializeField] private string Reference;
    [SerializeField] private List<string> Curiosities = new List<string>();
}
