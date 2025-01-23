using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    public GameObject[] bubblePrefabs;
    public GameObject acidBubblePrefab;
    public GameObject bigBubblePrefab; // Nouvelle grosse bulle
    public Transform[] randomSpawnPoints;
    public Transform[] lineSpawnPoints;
    public Transform[] cornerSpawnPoints;

    public float timeBetweenWaves = 10f;
    private int currentWaveIndex = 0;
    private int totalBubblesSpawned = 0;
    public float acidBubbleChance = 0.3f;

    public ObjectPool objectPool;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        objectPool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        StartCoroutine(StartWaveRoutine());
    }

    private IEnumerator StartWaveRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            yield return StartCoroutine(SpawnRandomWave());
            currentWaveIndex++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private IEnumerator SpawnRandomWave()
    {
        int bubbleCount = Random.Range(5, 15);
        float spawnRate = Random.Range(0.5f, 2f);

        for (int i = 0; i < bubbleCount; i++)
        {
            totalBubblesSpawned++;

            if (totalBubblesSpawned % 75 == 0)
            {
                SpawnFromCorner(bigBubblePrefab);
            }
            else
            {
                int spawnType = Random.Range(0, 2);

                GameObject bubblePrefab = bubblePrefabs[Random.Range(0, bubblePrefabs.Length)];

                switch (spawnType)
                {
                    case 0:
                        SpawnRandom(bubblePrefab);
                        break;
                    case 1:
                        StartCoroutine(SpawnLine(bubblePrefab));
                        break;
                }
                /*if (spawnType == 0 && Random.value < acidBubbleChance)
                {
                    SpawnRandom(acidBubblePrefab);
                }*/
                /*else
                {
                    GameObject bubblePrefab = bubblePrefabs[Random.Range(0, bubblePrefabs.Length)];

                    switch (spawnType)
                    {
                        case 0:
                            SpawnRandom(bubblePrefab);
                            break;
                        case 1:
                            StartCoroutine(SpawnLine(bubblePrefab));
                            break;
                    }
                }*/
            }

            yield return new WaitForSeconds(1f / spawnRate);
        }
    }

    private void SpawnRandom(GameObject bubblePrefab)
    {
        Transform spawnPoint = randomSpawnPoints[Random.Range(0, randomSpawnPoints.Length)];

        if (objectPool != null)
        {
            GameObject bubble = objectPool.GetFromPool(bubblePrefab);
            bubble.transform.position = spawnPoint.position;
            bubble.SetActive(true);
            StartCoroutine(GravitateAroundPoint(bubble, spawnPoint.position));
        }
        else
        {
            Debug.LogError("ObjectPool n'est pas assigné !");
        }
    }

    private IEnumerator GravitateAroundPoint(GameObject bubble, Vector3 center)
    {
        float angle = 0;
        float rotationSpeed = 10f;
        float radius = 0.3f;

        while (bubble != null)
        {
            angle += Time.deltaTime * rotationSpeed;
            bubble.transform.position = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator SpawnLine(GameObject bubblePrefab)
    {
        Transform spawnPoint = lineSpawnPoints[Random.Range(0, lineSpawnPoints.Length)];
        Vector3 spawnPosition = spawnPoint.position;
        int direction = spawnPoint.position.x < 0 ? 1 : -1;

        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
            GameObject bubble = objectPool.GetFromPool(bubblePrefab);
            bubble.transform.position = spawnPosition;
            bubble.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direction * 1.5f, 0);
            spawnPosition += Vector3.up * 1.0f;
        }
        yield return null;
    }

    private void SpawnFromCorner(GameObject bubblePrefab)
    {
        Transform spawnPoint = cornerSpawnPoints[Random.Range(0, cornerSpawnPoints.Length)];
        GameObject bubble = objectPool.GetFromPool(bubblePrefab);
        bubble.transform.position = spawnPoint.position;
        StartCoroutine(MoveInCurve(bubble, spawnPoint.position));
    }

    private IEnumerator MoveInCurve(GameObject bubble, Vector3 startPosition)
    {
        float time = 0;
        float curveHeight = Random.Range(1.5f, 4f);
        Vector3 endPosition = new Vector3(0, startPosition.y + 4, startPosition.z);

        while (time < 1)
        {
            time += Time.deltaTime * 0.8f;
            float curvedY = Mathf.Lerp(startPosition.y, endPosition.y, time);
            float curvedX = startPosition.x * (1 - time);
            bubble.transform.position = new Vector3(curvedX, curvedY + Mathf.Sin(time * Mathf.PI) * curveHeight, startPosition.z);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
