using UnityEngine;
using UnityEngine.InputSystem;

public class TouchInputHandler : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private InputSystem_Actions touchControls;
    public GameObject MouseCursor;

    private void Awake()
    {
        touchControls = new InputSystem_Actions();
        MouseCursor = GameObject.Find("MouseCursor");
    }

    private void OnEnable()
    {
        touchControls.Enable();
        touchControls.Player.TouchAction.performed += ctx => HandleTouch(ctx);
        //touchControls.Player.MouseClickAction.performed += ctx => HandleMouseClickStart(ctx);
        touchControls.Player.MouseClickAction.canceled += ctx => HandleMouseClickEnd(ctx);
        Debug.Log("Touch and mouse click actions enabled and events registered.");
    }

    private void OnDisable()
    {
        touchControls.Player.TouchAction.performed -= ctx => HandleTouch(ctx);
        //touchControls.Player.MouseClickAction.started -= ctx => HandleMouseClickStart(ctx);
        touchControls.Player.MouseClickAction.canceled -= ctx => HandleMouseClickEnd(ctx);
        touchControls.Disable();
        
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Debug.Log($"Touched at: {mousePos}");

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
            MouseCursor.transform.position = worldPosition;
            MouseCursor.GetComponentInChildren<TrailRenderer>().emitting = true;

            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log($"Touched object: {hit.collider.gameObject.name}");

                if (hit.collider.CompareTag("bulle") || hit.collider.CompareTag("grosseBulle"))
                {
                    hit.collider.gameObject.SendMessage("OnTouched", SendMessageOptions.DontRequireReceiver);
                }
                else if (hit.collider.CompareTag("bulleAcide"))
                {
                    LoseLife();
                    //Destroy(hit.collider.gameObject);
                    hit.collider.gameObject.SendMessage("OnTouched", SendMessageOptions.DontRequireReceiver);

                }
            }
        }
        
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

            if (hit.collider.CompareTag("bulle") || hit.collider.CompareTag("grosseBulle"))
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

    public void HandleMouseClickStart(InputAction.CallbackContext context)
    {

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Debug.Log($"Mouse clicked at: {mousePosition}");

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log($"Touched object: {hit.collider.gameObject.name}");

            if (hit.collider.CompareTag("bulle") || hit.collider.CompareTag("grosseBulle"))
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
        MouseCursor.GetComponentInChildren<TrailRenderer>().emitting = false;
    }

    private void LoseLife()
    {
        GameManager.Instance.playerLife--;
        Debug.Log($"Vie perdue ! Vies restantes: {GameManager.Instance.playerLife}");
        GameManager.Instance.UpdateLifeUI();

        if (GameManager.Instance.playerLife <= 0)
        {
            Debug.Log("Game Over !");
        }
    }
}