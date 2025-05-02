using JetBrains.Annotations;
using UnityEditor.Recorder;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public bool canStartWave = true;

    private float spawnRate = 10f;
    [SerializeField] private float timeTillSpawn = 10f;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemiesPerWave = 4;
    public int enemiesLeftToSpawn;

    public int waveNumber = 1;

    private EnemiesController enemiesController;
    private WavesUI wavesUI;

    public void Start()
    {
        enemiesController = GetComponent<EnemiesController>();
        wavesUI = FindFirstObjectByType<WavesUI>();

        WaveStarts();
    }
    // Update is called once per frame
    void Update()
    {
        timeTillSpawn -= Time.deltaTime;

        if (canStartWave == true)
        {
            WaveStarts();
        }
    }
    public void WaveStarts()
    {
        enemiesLeftToSpawn = enemiesPerWave;
        if (timeTillSpawn <= 0 && enemiesLeftToSpawn >= 0)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemiesLeftToSpawn--;
            timeTillSpawn = spawnRate;
        }
        if (enemiesLeftToSpawn == 0)
        {
            waveNumber += 1;
            wavesUI.TextUpdate();
            canStartWave = false;

        }
    }
    public void startNextWave()
    {
        spawnRate -= waveNumber / 5;
        enemiesPerWave += waveNumber * 4;

        enemiesController.buffedDamage = enemiesController.baseDamage + waveNumber/2;
        enemiesController.buffedHP = enemiesController.baseHP + waveNumber;
    }
}
