using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetFromPool(GameObject prefab)
    {
        if (poolDictionary.ContainsKey(prefab) && poolDictionary[prefab].Count > 0)
        {
            GameObject pooledObject = poolDictionary[prefab].Dequeue();
            pooledObject.SetActive(true);
            return pooledObject;
        }
        else
        {
            GameObject newObj = Instantiate(prefab);
            return newObj;
    }
}

    public void ReturnToPool(GameObject prefab, GameObject instance)
    {
        instance.SetActive(false);
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
        }
        poolDictionary[prefab].Enqueue(instance);
        Destroy(instance);
    }
}
