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
        touchControls.Player.MouseClickAction.started += ctx => HandleMouseClickStart(ctx);
        touchControls.Player.MouseClickAction.canceled += ctx => HandleMouseClickEnd(ctx);
        Debug.Log("Touch and mouse click actions enabled and events registered.");
    }

    private void OnDisable()
    {
        touchControls.Player.TouchAction.performed -= ctx => HandleTouch(ctx);
        touchControls.Player.MouseClickAction.started -= ctx => HandleMouseClickStart(ctx);
        touchControls.Player.MouseClickAction.canceled -= ctx => HandleMouseClickEnd(ctx);
        touchControls.Disable();
    }

    private void HandleTouch(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = context.ReadValue<Vector2>();
        Debug.Log($"Touched at: {touchPosition}");

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log($"Touched object: {hit.collider.gameObject.name}");

            if (hit.collider.CompareTag("bulle"))
            {
                hit.collider.gameObject.SendMessage("OnTouched", SendMessageOptions.DontRequireReceiver);
            }
            else if (hit.collider.CompareTag("bulleAcide"))
            {
                LoseLife();
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void HandleMouseClickStart(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Debug.Log($"Mouse clicked at: {mousePosition}");

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log($"Touched object: {hit.collider.gameObject.name}");

            if (hit.collider.CompareTag("bulle"))
            {
                hit.collider.gameObject.SendMessage("OnTouched", SendMessageOptions.DontRequireReceiver);
            }
            else if (hit.collider.CompareTag("bulleAcide"))
            {
                LoseLife();
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void HandleMouseClickEnd(InputAction.CallbackContext context)
    {
        Debug.Log("Mouse button released.");
    }

    private void LoseLife()
    {
        GameManager.Instance.playerLife--;
        Debug.Log($"Vie perdue ! Vies restantes: {GameManager.Instance.playerLife}");

        if (GameManager.Instance.playerLife <= 0)
        {
            Debug.Log("Game Over !");
        }
    }
}