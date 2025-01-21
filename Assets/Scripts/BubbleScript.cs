using Unity.VisualScripting;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    public GameObject myPrefab;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("it's the limit of height"))
        {
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
    public void OnTouched()
    {
        Debug.Log($"Touched: {gameObject.name}");
        GetComponent<SpriteRenderer>().color = Color.red;
        GameManager.Instance.AddScore(1);
        ObjectPool.Instance.ReturnToPool(myPrefab, gameObject);
    }

}
