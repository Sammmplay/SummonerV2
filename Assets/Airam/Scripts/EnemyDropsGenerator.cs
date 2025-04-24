using UnityEngine;

public class EnemyDropsGenerator : MonoBehaviour
{
    [Header("Generator Settings")]
    [SerializeField]
    GameObject[] enemyDrops;
    [SerializeField]
    private float spawnChance;
    [SerializeField]
    private Vector3 spawnPositionTweak;

    public void DropsSpawn()
    {
        if (spawnChance > Random.Range(0, 101))
        {
            int dropsIndex = Random.Range(0, enemyDrops.Length);
            Vector3 dropPosition = transform.position + Vector3.one * enemyDrops[dropsIndex].transform.position.y + spawnPositionTweak;
            GameObject dropSpawned = Instantiate(enemyDrops[dropsIndex], dropPosition, Quaternion.identity);
        }
    }
}
