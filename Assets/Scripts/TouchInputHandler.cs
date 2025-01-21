using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TouchInputHandler : MonoBehaviour
{
    private InputSystem_Actions touchControls;

    private void Awake()
    {
        touchControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        touchControls.Enable();
        touchControls.Player.TouchAction.performed += ctx => HandleTouch(ctx);
    }

    private void OnDisable()
    {
        touchControls.Player.TouchAction.performed -= ctx => HandleTouch(ctx);
        touchControls.Disable();
    }

    private void HandleTouch(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = context.ReadValue<Vector2>();
        Debug.Log($"Touched at: {touchPosition}");

        // Exemple : Convertir la position en coordonnées du monde
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
    }
}
