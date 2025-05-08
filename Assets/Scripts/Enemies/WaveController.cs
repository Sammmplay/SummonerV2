using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    [Range(0f, 1f)] public float spawnProbability;
}

public class WaveController : MonoBehaviour
{
    public Playercontroller playerController;

    [Header("Wave Settings")]
    public bool canStartWave = true;
    public int enemiesPerWave = 4;
    public int defeatedEnemies;
    [SerializeField] private float spawnRate = 10f;
    private float timeTillSpawn;
    private int enemiesLeftToSpawn;
    public int waveNumber = 1;

    [Header("Enemy Types")]
    [SerializeField] private EnemySpawnData[] enemyTypes;

    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float minDistanceFromPlayer = 5f;
    [SerializeField] private Transform player;

    private List<Vector3> gridPositions = new List<Vector3>();

    [Header("References")]
    private EnemiesController enemiesController;
    private WavesUI wavesUI;

    void Start()
    {
        enemiesController = GetComponent<EnemiesController>();
        wavesUI = FindFirstObjectByType<WavesUI>();
        playerController = GetComponent<Playercontroller>();

        GenerateGrid();
        timeTillSpawn = spawnRate;
        WaveStarts();
    }

    void Update()
    {
        if (!canStartWave) return;

        timeTillSpawn -= Time.deltaTime;

        if (timeTillSpawn <= 0f && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            timeTillSpawn = spawnRate;
        }

        if ((enemiesLeftToSpawn == 0) && (defeatedEnemies == enemiesPerWave))
        {   
            canStartWave = false; //se termina la oleada
        }
    }

    void WaveStarts()
    {
        enemiesLeftToSpawn = enemiesPerWave;
    }

    void SpawnEnemy()
    {
        GameObject selectedEnemy = GetRandomEnemyPrefab();
        Vector3 playerPos = player.position;
        List<Vector3> validPositions = GetValidSpawnPositions(playerPos, minDistanceFromPlayer);

        if (validPositions.Count == 0)
        {
            Debug.LogWarning("No hay posiciones v�lidas para spawnear enemigos.");
            return;
        }

        Vector3 spawnPos = validPositions[Random.Range(0, validPositions.Count)];
        Instantiate(selectedEnemy, spawnPos, Quaternion.identity);
        enemiesLeftToSpawn--;
    }

    GameObject GetRandomEnemyPrefab()
    {
        float totalProbability = 0f;
        foreach (var enemy in enemyTypes)
        {
            totalProbability += enemy.spawnProbability;
        }

        float randomPoint = Random.value * totalProbability;
        float currentSum = 0f;

        foreach (var enemy in enemyTypes)
        {
            currentSum += enemy.spawnProbability;
            if (randomPoint <= currentSum)
            {
                return enemy.enemyPrefab;
            }
        }

        return enemyTypes[0].enemyPrefab;
    }

    List<Vector3> GetValidSpawnPositions(Vector3 playerPosition, float minDistance)
    {
        List<Vector3> validPositions = new List<Vector3>();

        foreach (var pos in gridPositions)
        {
            if (Vector3.Distance(pos, playerPosition) >= minDistance)
            {
                validPositions.Add(pos);
            }
        }

        return validPositions;
    }

    void GenerateGrid()
    {
        gridPositions.Clear();

        // Centra el grid alrededor de (0,0)
        float originX = (gridWidth / 2f - 0.5f) * cellSize;
        float originZ = (gridHeight / 2f - 0.5f) * cellSize;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                float posX = (x * cellSize) - originX;
                float posZ = (z * cellSize) - originZ;
                Vector3 pos = new Vector3(posX, 0, posZ);
                gridPositions.Add(pos);
            }
        }
    }

    public void startNextWave()
    {
        waveNumber++;
        playerController.currentCharacterHP = playerController.characterHP;
        wavesUI.TextUpdate();
        spawnRate = Mathf.Max(1f, spawnRate - waveNumber / 5f);
        enemiesPerWave += waveNumber * 4;

        //stats aumentadas
        enemiesController.buffedDamage += enemiesController.buffedDamage * 10/100;
        enemiesController.buffedHP += enemiesController.buffedHP * 10 / 100;
    }

    // Visualiza el grid en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        float originX = (gridWidth / 2f - 0.5f) * cellSize;
        float originZ = (gridHeight / 2f - 0.5f) * cellSize;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                float posX = (x * cellSize) - originX;
                float posZ = (z * cellSize) - originZ;
                Vector3 pos = new Vector3(posX, 0, posZ);
                Gizmos.DrawWireCube(pos, Vector3.one * cellSize * 0.9f);
            }
        }
    }
}
