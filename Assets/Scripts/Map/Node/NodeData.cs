using UnityEngine;
public enum NodeType
{
    Combat,
    Event,
    Shop,
    Rest,
    Elite,
    Boss
}

[CreateAssetMenu(fileName = "NodeData", menuName = "Scriptable Objects/NodeData")]
public class NodeData : ScriptableObject
{
    public string NodeName = "Default";
    public NodeType Type;
    public Sprite Icon;
    public Color Color = Color.white;
}
