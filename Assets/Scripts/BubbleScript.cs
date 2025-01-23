using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 moveDirection;
    private Animator bubbleAnim;

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
    }

    private void Awake()
    {
        bubbleAnim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        bubbleAnim = GetComponent<Animator>();
    }
    private void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    public void OnTouched()
    {
        if (CompareTag("bulle"))
        {
            Debug.Log($"Bulle normale touchée: {gameObject.name}");
            GameManager.Instance.AddScore(1);
            //Destroy(gameObject);
            bubbleAnim.Play("bubblepop");
            GetComponent<CircleCollider2D>().enabled = false;

        }
        else if (CompareTag("bulleAcide"))
        {
            GameManager.Instance.AddScore(-1);
            //Destroy(gameObject);
            bubbleAnim.Play("bubblepop");
            GetComponent<CircleCollider2D>().enabled = false;

        }
        else if (CompareTag("grosseBulle"))
        {
            GameManager.Instance.AddScore(5);
            //Destroy(gameObject);
            bubbleAnim.Play("bubblepop");
            GetComponent<CircleCollider2D>().enabled = false;

        }
    }
    
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
