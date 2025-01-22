using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    public GameObject[] bubblePrefabs;  // Différents types de bulles
    public Transform[] randomSpawnPoints;  // Points pour spawn aléatoirement
    public Transform[] lineSpawnPoints;    // Points pour les lignes
    public Transform[] cornerSpawnPoints;  // Coins inférieurs pour spawn en courbe

    public float timeBetweenWaves = 10f;  // Délai entre vagues
    private int currentWaveIndex = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(StartWaveRoutine());
    }

    private IEnumerator StartWaveRoutine()
    {
        yield return new WaitForSeconds(2f);  // Délai avant la première vague

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
        GameObject bubblePrefab = bubblePrefabs[Random.Range(0, bubblePrefabs.Length)];
        int spawnType = Random.Range(0, 3); // 0 = random, 1 = line, 2 = corner

        for (int i = 0; i < bubbleCount; i++)
        {
            switch (spawnType)
            {
                case 0:
                    SpawnRandom(bubblePrefab);
                    break;
                case 1:
                    SpawnLine(bubblePrefab);
                    break;
                case 2:
                    SpawnFromCorner(bubblePrefab);
                    break;
            }
            yield return new WaitForSeconds(1f / spawnRate);
        }
    }

    private void SpawnRandom(GameObject bubblePrefab)
    {
        Transform spawnPoint = randomSpawnPoints[Random.Range(0, randomSpawnPoints.Length)];
        GameObject bubble = Instantiate(bubblePrefab, spawnPoint.position, Quaternion.identity);
        StartCoroutine(GravitateAroundPoint(bubble, spawnPoint.position));
    }

    private IEnumerator GravitateAroundPoint(GameObject bubble, Vector3 center)
    {
        float angle = 0;
        float rotationSpeed = 20f; // Réduction de la vitesse de rotation
        float radius = 0.5f; // Réduction de la distance au centre

        while (bubble != null)
        {
            angle += Time.deltaTime * rotationSpeed;
            bubble.transform.position = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            yield return null;
        }
    }

    private void SpawnLine(GameObject bubblePrefab)
    {
        Transform spawnPoint = lineSpawnPoints[Random.Range(0, lineSpawnPoints.Length)];
        Vector3 spawnPosition = spawnPoint.position;
        int direction = spawnPoint.position.x < 0 ? 1 : -1; // Gauche -> droite, Droite -> gauche

        for (int i = 0; i < 5; i++) // Ligne de 5 bulles
        {
            GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);
            bubble.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direction * 2f, 0);
            spawnPosition += Vector3.up * 1.2f; // Espacement entre bulles
        }
    }

    private void SpawnFromCorner(GameObject bubblePrefab)
    {
        Transform spawnPoint = cornerSpawnPoints[Random.Range(0, cornerSpawnPoints.Length)];
        GameObject bubble = Instantiate(bubblePrefab, spawnPoint.position, Quaternion.identity);
        StartCoroutine(MoveInCurve(bubble, spawnPoint.position));
    }

    private IEnumerator MoveInCurve(GameObject bubble, Vector3 startPosition)
    {
        float time = 0;
        float curveHeight = Random.Range(2f, 5f);
        Vector3 endPosition = new Vector3(0, startPosition.y + 5, startPosition.z);

        while (time < 1)
        {
            time += Time.deltaTime;
            float curvedY = Mathf.Lerp(startPosition.y, endPosition.y, time);
            float curvedX = startPosition.x * (1 - time);  // Courbe vers le centre
            bubble.transform.position = new Vector3(curvedX, curvedY + Mathf.Sin(time * Mathf.PI) * curveHeight, startPosition.z);
            yield return null;
        }
    }
}