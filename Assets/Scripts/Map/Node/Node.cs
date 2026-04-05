using UnityEngine;
using System.Collections.Generic;
using System;

public enum NodeState
{
    Active,
    Inactive,
    Completed,
    Deactivated
}

public class Node : MonoBehaviour 
{
    [SerializeField] private Transform nodePosition;
    [SerializeField] private NodeData NodeData;
    [SerializeField] private GameObject OutterRing;
    [SerializeField] private GameObject Panel;

    private NodeState State = NodeState.Inactive;

    private int Layer;

    private List<GameObject> NextNodes = new List<GameObject>();

    public void SetLayer(int layer)
    {
        Layer = layer;
    }

    public void UpdatePosition(Vector2 position)
    {
        nodePosition.position = position;
    }

    public void UpdatePosition(float x, float y)
    {
        nodePosition.position = new Vector2(x, y);
    }

    public void AddNext(GameObject node)
    {
        if (!node) return;

        if (NextNodes.Contains(node)) return;

        NextNodes.Add(node);
    }

    public float NodeDistance(GameObject node)
    {
        float x1 = nodePosition.position.x;
        float y1 = nodePosition.position.y;

        float x2 = node.transform.position.x;
        float y2 = node.transform.position.y;

        float xDifference = Mathf.Pow((x2 - x1), 2);
        float yDifference = Mathf.Pow((y2 - y1), 2);

        return Mathf.Sqrt(xDifference + yDifference);
    }

    public NodeData GetNodeData()
    {
        return NodeData;
    }

    public void SetNodeData(NodeData nodeData)
    {
        NodeData = nodeData;
    }

    public NodeState GetState()
    {
        return State;
    }

    private SpriteRenderer GetSpriteRenderer()
    {
        return GetComponent<SpriteRenderer>();
    }

    public void SetPanel(GameObject panel)
    {
        Panel = panel;
    }

    public void OpenPanel()
    {
        Panel.SetActive(true);
    }

    public void Activate()
    {
        State = NodeState.Active;

        SpriteRenderer spriteRenderer = GetSpriteRenderer();

        Color newColor = spriteRenderer.color;
        newColor.a = 1f;

        spriteRenderer.color = newColor;
    }

    public void Deactivate()
    {
        State = NodeState.Deactivated;

        SpriteRenderer spriteRenderer = GetSpriteRenderer();

        Color newColor = Color.red;
        newColor.a = 0.7f;

        spriteRenderer.color = newColor;
    }

    public void Complete()
    {
        foreach(GameObject NodeObj in NextNodes)
        {
            NodeObj.GetComponent<Node>().Activate();
        }

        MapController mapController = FindFirstObjectByType<MapController>();
        if(mapController) mapController.DeactivateLayer(Layer);

        State = NodeState.Completed;

        SpriteRenderer spriteRenderer = GetSpriteRenderer();

        Color newColor = Color.green;
        newColor.a = 0.8f;

        spriteRenderer.color = newColor;
    }

    public void ActivateOutterRing()
    {
        OutterRing.SetActive(true);
    }

    public void DeactivateOutterRing()
    {
        OutterRing.SetActive(false);
    }
}

