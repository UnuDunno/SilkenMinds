using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Effect
{
    public CardEffect cardEffect;
    public int value;
}


[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string cardDescription;
    public Sprite cardImage;
    public int fearValue;
    public List<Effect> effects;
}
