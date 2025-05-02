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

    /// <summary>
    /// Función que instancia recursos al crearse el grid, de forma aleatoria
    /// </summary>
    private void ResourcesSpawn()
    {
        if (spawnChance > Random.Range(0, 101))
        {
            int resourcesIndex = Random.Range(0, environmentResources.Length);
            Vector3 resourcePosition = transform.position + Vector3.one * environmentResources[resourcesIndex].transform.position.y + spawnPositionTweak;
            GameObject resourceSpawned = Instantiate(environmentResources[resourcesIndex], resourcePosition, Quaternion.identity, transform);     
        }
    }
}
