using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    public GameObject myPrefab;

    public float speed = 2f;
    private Vector3 moveDirection;
    public ObjectPool poolingSystem;

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
        Debug.Log($"Touched: {gameObject.name}");
        GetComponent<SpriteRenderer>().color = Color.red;
        GameManager.Instance.AddScore(1);
        Destroy(gameObject);
    }

}