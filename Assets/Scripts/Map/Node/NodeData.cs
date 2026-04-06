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
    public string nodeName = "Default";
    public NodeType type;
    public Sprite icon;
    public Color color = Color.white;
}
