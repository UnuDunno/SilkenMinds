using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public static MapCreator Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject NodePrefab;
    [SerializeField] private GameObject LinePrefab;

    [Header("Spawners")]
    [SerializeField] private GameObject NodesContainer;
    [SerializeField] private GameObject EdgesContainer;

    [Header("Panels")]
    [SerializeField] private GameObject CombatPanel;
    [SerializeField] private GameObject RestPanel;
    [SerializeField] private GameObject ShopPanel;
    [SerializeField] private GameObject EventPanel;

    private readonly List<(int weight, NodeType type)> NodesWeights = new List<(int, NodeType)> {
        (7, NodeType.Combat),
        (5, NodeType.Rest),
        (4, NodeType.Shop),
        (7, NodeType.Event),
        (1, NodeType.Elite)
    };

    void Awake()
    {
        if (Instance != null && Instance != this) { 
            Destroy(gameObject); 
        }
        else { 
            Instance = this; 
        }
    }

    public void GenerateMap(List<List<GameObject>> map, float initialRadius, int layers, int layerMultiplier, float radiusIncrement)
    {
        GenerateMapNodes(map, initialRadius, layers, layerMultiplier, radiusIncrement);
        GenerateMapEdges(map);
    }

    private void GenerateMapNodes(List<List<GameObject>> map, float initialRadius, int layers, int layerMultiplier, float radiusIncrement)
    {
        if (!NodePrefab) return;

        Vector2 mapCenter = new Vector2(0, 0);

        float radius = initialRadius;
        int nodesPerLayer;
        bool lastLayer;
        for (int layer = 0; layer < layers; layer++)
        {
            lastLayer = layer == layers - 1;

            List<GameObject> currentLayer = new List<GameObject>();

            if (layer == 0)
            {
                GameObject newNode = CreateNode(mapCenter, NodeType.Boss, layer);

                currentLayer.Add(newNode);
            }
            else
            {
                nodesPerLayer = layer * layerMultiplier;
                radius += (layer * radiusIncrement);

                Vector2[] positions = CalculateNodesPositions(mapCenter, radius, nodesPerLayer);
                foreach (Vector2 position in positions)
                {
                    GameObject newNode = CreateNode(position, GetNodeType(), layer);

                    if(lastLayer) newNode.GetComponent<Node>().Activate();

                    currentLayer.Add(newNode);
                }
            }
            map.Add(currentLayer);
        }
    }

    private Vector2[] CalculateNodesPositions(Vector2 centerPosition, float radius, int numberOfNodes)
    {
        Vector2[] points = new Vector2[numberOfNodes];
        float angleStep = (2 * Mathf.PI) / numberOfNodes;

        for (int i = 1; i <= numberOfNodes; i++)
        {
            float currentAngle = i * angleStep;

            float x = centerPosition.x + radius * Mathf.Cos(currentAngle);
            float y = centerPosition.y + radius * Mathf.Sin(currentAngle);

            points[i - 1] = new Vector2(x, y);
        }

        return points;
    }

    private void GenerateMapEdges(List<List<GameObject>> map)
    {
        float connectionPercentage;
        for (int layer = map.Count - 1; layer > 0; layer--)
        {
            connectionPercentage = layer / (map.Count - 1) + 0.3f;

            List<GameObject> currentLayer = map[layer];

            ConnectLayer(currentLayer, connectionPercentage);

            List<GameObject> nextLayer = map[layer - 1];

            ConnectNextLayer(currentLayer, nextLayer, connectionPercentage);
        }
    }

    private void ConnectLayer(List<GameObject> layer, float connectionPercentage)
    {
        float randomValue = UnityEngine.Random.value;

        GameObject currentNode, nextNode;
        for (int i = 0; i < layer.Count; i++)
        {
            currentNode = layer[i];
            nextNode = (i == layer.Count - 1) ? layer[0] : layer[i + 1];

            if (randomValue <= connectionPercentage)
            {
                CreateEdge(currentNode, nextNode);
                currentNode.GetComponent<Node>().AddNext(currentNode);
            }
        }
    }

    private void ConnectNextLayer(List<GameObject> currentLayer, List<GameObject> nextLayer, float connectionPercentage)
    {
        float randomValue = UnityEngine.Random.value;

        GameObject currentLayerNode, nextLayerNextNode1, nextLayerNextNode2;
        List<GameObject> OrderedNextLayer;
        for (int i = 0; i < currentLayer.Count; i++)
        {
            currentLayerNode = currentLayer[i];

            OrderedNextLayer = nextLayer.OrderBy(o => o.GetComponent<Node>().NodeDistance(currentLayerNode)).ToList();

            nextLayerNextNode1 = OrderedNextLayer[0];
            nextLayerNextNode2 = (OrderedNextLayer.Count > 1) ? OrderedNextLayer[1] : OrderedNextLayer[0];

            CreateEdge(currentLayerNode, nextLayerNextNode1);
            currentLayerNode.GetComponent<Node>().AddNext(nextLayerNextNode1);

            if (randomValue <= connectionPercentage)
            {
                CreateEdge(currentLayerNode, nextLayerNextNode2);
                currentLayerNode.GetComponent<Node>().AddNext(nextLayerNextNode2);
            }
        }
    }

    private NodeType GetNodeType()
    {
        int nodesWeightsSize = NodesWeights.Count;
        int totalWeight = 0;

        foreach(var (weight, type) in NodesWeights)
        {
            totalWeight += weight;
        }

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for(int i = 0; i < nodesWeightsSize; i++)
        {
            cumulative += NodesWeights[i].weight;

            if(roll < cumulative)
            {
                return NodesWeights[i].type;
            }
        }

        return NodesWeights[nodesWeightsSize - 1].type;
    }

    private NodeData CreateNodeData(NodeType nodeType)
    {
        if (nodeType == NodeType.Combat) return Resources.Load<NodeData>("NodeData/CombatNode");
        if (nodeType == NodeType.Elite) return Resources.Load<NodeData>("NodeData/EliteNode");
        if (nodeType == NodeType.Event) return Resources.Load<NodeData>("NodeData/EventNode");
        if (nodeType == NodeType.Rest) return Resources.Load<NodeData>("NodeData/RestNode");
        if (nodeType == NodeType.Boss) return Resources.Load<NodeData>("NodeData/BossNode");
        if (nodeType == NodeType.Shop) return Resources.Load<NodeData>("NodeData/ShopNode");

        return Resources.Load<NodeData>("NodeData/CombatNode");
    }

    private GameObject CreateNode(Vector2 position, NodeType nodeType, int layer)
    {
        GameObject newNode = Instantiate(NodePrefab, NodesContainer.transform);
        Node node = newNode.GetComponent<Node>();

        NodeData nodeData = CreateNodeData(nodeType);

        node.UpdatePosition(position);
        node.SetNodeData(nodeData);
        node.SetLayer(layer);

        switch(nodeType)
        {
            case NodeType.Combat:
            case NodeType.Elite:
            case NodeType.Boss:
                node.SetPanel(CombatPanel);
                break;
            case NodeType.Event:
                node.SetPanel(EventPanel);
                break;
            case NodeType.Rest:
                node.SetPanel(RestPanel);
                break;
            case NodeType.Shop:
                node.SetPanel(ShopPanel);
                break;
        }

        SpriteRenderer nodeRenderer = newNode.GetComponent<SpriteRenderer>();
        nodeRenderer.sprite = nodeData.Icon;
        nodeRenderer.color = nodeData.Color;

        return newNode;
    }

    private void CreateEdge(GameObject a, GameObject b)
    {
        if (!LinePrefab) return;

        GameObject edge = Instantiate(LinePrefab, EdgesContainer.transform);
        LineRenderer lr = edge.GetComponent<LineRenderer>();

        lr.SetPosition(0, a.transform.position);
        lr.SetPosition(1, b.transform.position);
    }
}
