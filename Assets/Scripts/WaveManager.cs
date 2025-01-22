using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    public Wave[] waves; // Waves list
    public Transform[] spawnPoints; // Bubbles spawn point
    public float timeBetweenWaves = 10f; // delay between waves

    public int currentWaveIndex = 0;
    [SerializeField] private bool isWaveActive = false;
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

    private IEnumerator StartWaveRoutine() //start the manager
    {
        yield return new WaitForSeconds(2f); // Waiting before the first wave

        while (currentWaveIndex < waves.Length)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            currentWaveIndex++;

            // Increase difficulty after each wave
            //IncreaseDifficulty();

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("Toutes les vagues sont terminées !");
    }

    private IEnumerator SpawnWave(Wave wave) //spawn a wave
    {
        isWaveActive = true;
        BubblesInTheWave = wave.bubbleCount;
        Debug.Log("Lancement de la vague : " + wave.waveName);

        for (int i = 0; i < wave.bubbleCount; i++)
        {
            SpawnBubble(wave.prefab);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        isWaveActive = false;
    }

    private void SpawnBubble(GameObject BubblePrefab) //spawn a bubble
    {
        Debug.Log("je spawn une bulle");
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject bubble = ObjectPool.Instance.GetFromPool(BubblePrefab);
        bubble.transform.position = spawnPoint.position;
        bubble.SetActive(true);
    }



    private void IncreaseDifficulty()
    {
        foreach (Wave wave in waves)
        {
            wave.bubbleCount += 2; // Increase the number of enemies per wave
            wave.spawnRate += 0.1f; // Reduce time between spawns
        }

        Debug.Log("Difficulté augmentée !");
    }

}
