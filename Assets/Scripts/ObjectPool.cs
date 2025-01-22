using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject[] bubblePrefabs;
    public int poolSize = 10;
    private Queue<GameObject> bubblePool;
    private ObjectPool objectPool;

    private void Awake()
    {
        objectPool = GetComponent<ObjectPool>();

        if (objectPool == null)
        {
            Debug.LogError("ObjectPool non trouvé dans la scène !");
        }
    }
    private void Start()
    {
        bubblePool = new Queue<GameObject>();
        foreach (var prefab in bubblePrefabs)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject bubble = Instantiate(prefab);
                bubble.SetActive(false);
                bubblePool.Enqueue(bubble);
            }
        }
    }

    public GameObject GetFromPool(GameObject prefab)
    {
        if (bubblePool.Count > 0)
        {
            GameObject bubble = bubblePool.Dequeue();
            bubble.SetActive(true);
            return bubble;
        }
        else
        {
            GameObject bubble = Instantiate(prefab);
            return bubble;
        }
    }

    public void ReturnToPool(GameObject bubble)
    {
        bubble.SetActive(false);
        bubblePool.Enqueue(bubble);
    }
}
