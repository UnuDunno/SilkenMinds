using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDrag : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Difference;

    private Camera MainCamera;

    private bool IsDragging;

    private void Awake()
    {
        MainCamera = Camera.main;
    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            Origin = GetMousePosition();
        }

        IsDragging = ctx.started || ctx.performed;
    }

    private void LateUpdate()
    {
        if (!IsDragging) return;

        Difference = GetMousePosition() - transform.position;
        transform.position = Origin - Difference;
    }

    private Vector3 GetMousePosition()
    {
        return MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
