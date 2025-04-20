using UnityEngine;

public class ResourcesGenerator : MonoBehaviour
{
    [Header("Generator Settings")]
    [SerializeField]
    GameObject[] environmentResources;
    [SerializeField]
    private float spawnChance;
    [SerializeField]
    private Vector3 spawnPositionTweak;

    private void Start()
    {
        ResourcesSpawn();
    }

    private void ResourcesSpawn()
    {
        if (spawnChance > Random.Range(0, 101))
        {
            int resourcesIndex = Random.Range(0, environmentResources.Length);
            GameObject resourceSpawned = Instantiate(environmentResources[resourcesIndex], transform.position + spawnPositionTweak, Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 0f)), transform);     
        }
    }
}
