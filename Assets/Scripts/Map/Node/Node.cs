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
    [SerializeField] private NodeData nodeData;
    [SerializeField] private GameObject outterRing;
    [SerializeField] private GameObject panel;

    private PanelController panelController;

    private NodeState state = NodeState.Inactive;

    private int layer;

    private List<GameObject> nextNodes = new List<GameObject>();

    public void SetLayer(int layer)
    {
        this.layer = layer;
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

        if (nextNodes.Contains(node)) return;

        nextNodes.Add(node);
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
        return nodeData;
    }

    public void SetNodeData(NodeData nodeData)
    {
        this.nodeData = nodeData;
    }

    public NodeState GetState()
    {
        return state;
    }

    private SpriteRenderer GetSpriteRenderer()
    {
        return GetComponent<SpriteRenderer>();
    }

    public void SetPanel(GameObject panel)
    {
        this.panel = panel;

        panelController = panel.GetComponent<PanelController>();

    }

    public void OpenPanel()
    {
        panelController.ResetPanel(nodeData.type);
        panel.SetActive(true);
    }

    public void Activate()
    {
        state = NodeState.Active;

        SpriteRenderer spriteRenderer = GetSpriteRenderer();

        Color newColor = spriteRenderer.color;
        newColor.a = 1f;

        spriteRenderer.color = newColor;
    }

    public void Deactivate()
    {
        state = NodeState.Deactivated;

        SpriteRenderer spriteRenderer = GetSpriteRenderer();

        Color newColor = Color.red;
        newColor.a = 0.7f;

        spriteRenderer.color = newColor;
    }

    public void Complete()
    {
        foreach(GameObject NodeObj in nextNodes)
        {
            NodeObj.GetComponent<Node>().Activate();
        }

        MapController mapController = FindFirstObjectByType<MapController>();
        if(mapController) mapController.DeactivateLayer(layer);

        state = NodeState.Completed;

        SpriteRenderer spriteRenderer = GetSpriteRenderer();

        Color newColor = Color.green;
        newColor.a = 0.8f;

        spriteRenderer.color = newColor;
    }

    public void ActivateOutterRing()
    {
        outterRing.SetActive(true);
    }

    public void DeactivateOutterRing()
    {
        outterRing.SetActive(false);
    }
}

