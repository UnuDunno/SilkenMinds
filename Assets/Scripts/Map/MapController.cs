using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour 
{
    [SerializeField] private List<List<GameObject>> Map;

    [Header("Map Generation Configs")]
    [SerializeField] private int layers = 10;
    [SerializeField] private int layerMultiplier = 2;
    [SerializeField] private float initialRadius = 1f;
    [SerializeField] private float radiusIncrement = 1f;

    [Header("Map Generator")]
    [SerializeField] private MapCreator mapCreator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        Map = new List<List<GameObject>>();
        mapCreator.GenerateMap(Map, initialRadius, layers, layerMultiplier, radiusIncrement);
    }

    // Update is called once per frame
    void Update() 
    {
        
    }

    public void DeactivateLayer(int layer)
    {
        foreach (GameObject NodeObj in Map[layer])
        {
            NodeObj.GetComponent<Node>().Deactivate();
        }
    }
}
