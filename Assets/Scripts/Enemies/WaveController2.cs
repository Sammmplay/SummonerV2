using UnityEngine;

public class WaveController2 : MonoBehaviour
{
    public bool canStartWave = true;

    public float spawnRate = 10f;
    private float timeTillNextSpawn;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints; // <- Los 4 puntos de spawn

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

    void SpawnEnemy()
    {
        // Selecciona el spawn point de forma cíclica
        Transform spawnPoint = spawnPoints[spawnPointIndex];
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        enemiesLeftToSpawn--;

        // Alterna al siguiente spawn point
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