using UnityEngine;

public class WaveController2 : MonoBehaviour
{
    public bool canStartWave = true;

    public float spawnRate = 10f;
    private float timeTillNextSpawn;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints; // <- Los 4 puntos de spawn

    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        [Range(0f, 1f)] public float spawnProbability; // Entre 0 y 1
    }
    [SerializeField] private EnemySpawnData[] enemyTypes;

    [SerializeField] private int enemiesPerWave = 4;
    private int enemiesLeftToSpawn;

    public int waveNumber = 1;

    private EnemiesController enemiesController;
    private WavesUI wavesUI;

    private int spawnPointIndex = 0; // <- Para alternar los spawn points

    void Start()
    {
        enemiesController = GetComponent<EnemiesController>();
        wavesUI = FindFirstObjectByType<WavesUI>();

        StartWave();
    }

    void Update()
    {
        if (!canStartWave)
        {
            timeTillNextSpawn -= Time.deltaTime;

            if (timeTillNextSpawn <= 0 && enemiesLeftToSpawn > 0)
            {
                SpawnEnemy();
                timeTillNextSpawn = spawnRate;
            }

            if (enemiesLeftToSpawn <= 0)
            {
                // Fin de la oleada
                canStartWave = true;

                StartNextWave();
            }
        }
    }

    public void StartWave()
    {
        enemiesLeftToSpawn = enemiesPerWave;
        timeTillNextSpawn = 0f; // Spawnea el primero de inmediato
        canStartWave = false;
    }

    GameObject GetRandomEnemyPrefab()
    {
        float totalProbability = 0f;
        foreach (var enemy in enemyTypes)
        {
            totalProbability += enemy.spawnProbability;
        }

        float randomPoint = Random.value * totalProbability; // Random entre 0 y totalProbability
        float currentSum = 0f;

        foreach (var enemy in enemyTypes)
        {
            currentSum += enemy.spawnProbability;
            if (randomPoint <= currentSum)
            {
                return enemy.enemyPrefab;
            }
        }

        // Por seguridad, si algo falla, devuelve el primero
        return enemyTypes[0].enemyPrefab;
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[spawnPointIndex];
        GameObject selectedEnemy = GetRandomEnemyPrefab();

        Instantiate(selectedEnemy, spawnPoint.position, Quaternion.identity);

        enemiesLeftToSpawn--;
        spawnPointIndex = (spawnPointIndex + 1) % spawnPoints.Length;
    }

    public void StartNextWave()
    {
        waveNumber++;
        wavesUI.TextUpdate();
        spawnRate = Mathf.Max(2f, spawnRate - waveNumber / 3f); // Nunca menos de 2s
        enemiesPerWave += waveNumber * 2;

        enemiesController.buffedDamage = enemiesController.baseDamage + waveNumber / 2;
        enemiesController.buffedHP = enemiesController.baseHP + waveNumber;

        StartWave();
    }
}