using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapController : MonoBehaviour 
{
    [Header("Map Generation Configs")]
    [SerializeField] private MapCreator mapCreator;
    [SerializeField] private int layers = 10;
    [SerializeField] private int layerMultiplier = 2;
    [SerializeField] private float initialRadius = 1f;
    [SerializeField] private float radiusIncrement = 1f;

    [Header("Map Controls")]
    [SerializeField] private Camera mainCamera;

    //"---  Zoom  ---"
    private float targetZoom;
    [SerializeField] private float zoomSensitivity = 0.5f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 30f;
    [SerializeField] private float smoothSpeed = 10f;

    //"---  Drag  ---"
    private Vector3 origin;
    private Vector3 difference;
    private bool isDragging;

    private List<List<GameObject>> map;

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        targetZoom = mainCamera.orthographicSize;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        map = new List<List<GameObject>>();
        mapCreator.GenerateMap(map, initialRadius, layers, layerMultiplier, radiusIncrement);
    }

    // Update is called once per frame
    void Update() 
    {
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, Time.deltaTime * smoothSpeed);
    }

    void LateUpdate()
    {
        if (!isDragging) return;

        difference = GetMousePosition() - mainCamera.transform.position;
        mainCamera.transform.position = origin - difference;
    }

    public void OnZoom(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();

        targetZoom -= value.y * zoomSensitivity;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started) origin = GetMousePosition();

        isDragging = ctx.started || ctx.performed;
    }

    private Vector3 GetMousePosition()
    {
        return mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public void DeactivateLayer(int layer)
    {
        foreach (GameObject NodeObj in map[layer])
        {
            NodeObj.GetComponent<Node>().Deactivate();
        }
    }
}
