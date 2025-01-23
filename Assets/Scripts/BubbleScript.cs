using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 moveDirection;

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
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
            Destroy(gameObject);
        }
        else if (CompareTag("bulleAcide"))
        {
            GameManager.Instance.AddScore(-1);
            Destroy(gameObject);
        }
    }
}
