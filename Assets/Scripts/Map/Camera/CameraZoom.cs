using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private Camera MainCamera;
    private float TargetZoom;

    [Header("Settings")]
    [SerializeField] private float ZoomSensitivity = 0.5f;
    [SerializeField] private float MinZoom = 2f;
    [SerializeField] private float MaxZoom = 30f;
    [SerializeField] private float SmoothSpeed = 10f;

    private void Awake()
    {
        MainCamera = Camera.main;
        TargetZoom = MainCamera.orthographicSize;
    }

    public void OnZoom(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();

        TargetZoom -= value.y * ZoomSensitivity;
        TargetZoom = Mathf.Clamp(TargetZoom, MinZoom, MaxZoom);
    }

    private void Update()
    {
        MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, TargetZoom, Time.deltaTime * SmoothSpeed);
    }

    //void Update()
    //{
    //    float scroll = Input.GetAxis("Mouse ScrollWheel");
    //    Zoom -= scroll * ZoomMultiplier;
    //    Zoom = Mathf.Clamp(Zoom, minZoom, maxZoom);

    //    MainCamera.orthographicSize = Mathf.SmoothDamp(MainCamera.orthographicSize, Zoom, ref velocity, smoothTime);
    //}
}
