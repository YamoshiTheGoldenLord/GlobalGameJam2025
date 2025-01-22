using UnityEngine;
using UnityEngine.InputSystem;
public class TouchInputHandler : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private InputSystem_Actions touchControls;

    private void Awake()
    {

        touchControls = new InputSystem_Actions();
        

    }

    private void OnEnable()
    {
        touchControls.Enable();
        touchControls.Player.TouchAction.performed += ctx => HandleTouch(ctx);
        Debug.Log("Touch action enabled and event registered.");
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

        // Convert screen position to world coordinates
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));

        // Perform a raycast to detect collisions with 2D objects
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log($"Touched object: {hit.collider.gameObject.name}");

            // Execute a specific action on the touched object (ex: method call)
            hit.collider.gameObject.SendMessage("OnTouched", SendMessageOptions.DontRequireReceiver);
        }
    }
}
