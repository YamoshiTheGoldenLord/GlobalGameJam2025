using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    public Wave[] waves; // Waves list
    public Transform[] spawnPoints; // Bubbles spawn point
    public float timeBetweenWaves = 10f; // delay between waves

    public int currentWaveIndex = 0;
    private bool isWaveActive = false;
    private int BubblesInTheWave = 0;

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
        yield return new WaitForSeconds(2f); // Waiting before the first wave

        while (currentWaveIndex < waves.Length)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            currentWaveIndex++;

            // Increase difficulty after each wave
            IncreaseDifficulty();

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("Toutes les vagues sont terminées !");
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        isWaveActive = true;
        BubblesInTheWave = wave.enemyCount;
        Debug.Log("Lancement de la vague : " + wave.waveName);

        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnBubble(wave.enemyPrefab);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        isWaveActive = false;
    }

    private void SpawnBubble(GameObject BubblePrefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject bubble = ObjectPool.Instance.GetFromPool(BubblePrefab);
        bubble.transform.position = spawnPoint.position;
        bubble.SetActive(true);
    }

    private void IncreaseDifficulty()
    {
        foreach (Wave wave in waves)
        {
            wave.enemyCount += 2; // Augmenter le nombre d'ennemis par vague
            wave.spawnRate += 0.1f; // Réduire le temps entre les apparitions
        }

        Debug.Log("Difficulté augmentée !");
    }

}
