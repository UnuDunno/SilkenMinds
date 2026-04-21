using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public static MapCreator Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject linePrefab;

    [Header("Spawners")]
    [SerializeField] private GameObject nodesContainer;
    [SerializeField] private GameObject edgesContainer;

    [Header("Panels")]
    [SerializeField] private GameObject combatPanel;
    [SerializeField] private GameObject restPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject eventPanel;

    private readonly List<(int weight, NodeType type)> nodesWeights = new List<(int, NodeType)> {
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
        if (!nodePrefab) return;

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
        float randomValue;
        GameObject currentNode, nextNode;

        for (int i = 0; i < layer.Count; i++)
        {
            currentNode = layer[i];
            nextNode = (i == layer.Count - 1) ? layer[0] : layer[i + 1];

            randomValue = UnityEngine.Random.value;
            if (randomValue <= connectionPercentage)
            {
                CreateEdge(currentNode, nextNode);
                currentNode.GetComponent<Node>().AddNext(currentNode);
            }
        }
    }

    private void ConnectNextLayer(List<GameObject> currentLayer, List<GameObject> nextLayer, float connectionPercentage)
    {
        float randomValue;
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

            randomValue = UnityEngine.Random.value;
            if (randomValue <= connectionPercentage)
            {
                CreateEdge(currentLayerNode, nextLayerNextNode2);
                currentLayerNode.GetComponent<Node>().AddNext(nextLayerNextNode2);
            }
        }
    }

    private NodeType GetNodeType()
    {
        int nodesWeightsSize = nodesWeights.Count;
        int totalWeight = 0;

        foreach(var (weight, type) in nodesWeights)
        {
            totalWeight += weight;
        }

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for(int i = 0; i < nodesWeightsSize; i++)
        {
            cumulative += nodesWeights[i].weight;

            if(roll < cumulative)
            {
                return nodesWeights[i].type;
            }
        }

        return nodesWeights[nodesWeightsSize - 1].type;
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
        GameObject newNode = Instantiate(nodePrefab, nodesContainer.transform);
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
                node.SetPanel(combatPanel);
                break;
            case NodeType.Event:
                node.SetPanel(eventPanel);
                break;
            case NodeType.Rest:
                node.SetPanel(restPanel);
                break;
            case NodeType.Shop:
                node.SetPanel(shopPanel);
                break;
        }

        SpriteRenderer nodeRenderer = newNode.GetComponent<SpriteRenderer>();
        nodeRenderer.sprite = nodeData.icon;
        nodeRenderer.color = nodeData.color;

        return newNode;
    }

    private void CreateEdge(GameObject a, GameObject b)
    {
        if (!linePrefab) return;

        GameObject edge = Instantiate(linePrefab, edgesContainer.transform);
        LineRenderer lr = edge.GetComponent<LineRenderer>();

        lr.SetPosition(0, a.transform.position);
        lr.SetPosition(1, b.transform.position);
    }
}
