using UnityEngine;

public class ObjectDeletor : MonoBehaviour
{
    public ObjectPool poolSystem;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            poolSystem.ReturnToPool(collision.gameObject);
        }
    }
}
