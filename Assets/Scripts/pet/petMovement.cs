using System.Collections.Generic;
using UnityEngine;

public class PetMovement : MonoBehaviour
{
    [Header("Prefabs de Mascotas")]
    [SerializeField] private List<GameObject> prefabList = new List<GameObject>();

    [Header("Asignaciones Globales")]
    [SerializeField] private Transform player;

    [Header("Puntos de Formaciones")]
    [SerializeField] private List<Transform> defenderPoints;
    [SerializeField] private List<Transform> attackerPoints;
    [SerializeField] private List<Transform> assassinPoints;

    [Header("Parámetros de Combate Globales")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float maxDetectionRange = 10f;
    [SerializeField] private float minDetectionRange = 0f;

    [Header("Proyectiles y Tamaños")]
    [SerializeField] private GameObject attackerProjectile;
    [SerializeField] private float attackerProjectileScale = 0.3f;
    [SerializeField] private GameObject assassinProjectile;
    [SerializeField] private float assassinProjectileScale = 0.15f;
    [SerializeField] private GameObject defenderProjectile;
    [SerializeField] private float defenderProjectileScale = 0.5f;

    [Header("Parámetros de Spawn")]
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private float minScale = 0.3f;
    [SerializeField] private float maxScale = 0.6f;
    [SerializeField] private float spawnAnimationTime = 0.5f;
    [SerializeField] private float despawnAnimationTime = 0.5f;

    private List<IPetBehavior> petBehaviors = new List<IPetBehavior>();
    private List<GameObject> petInstances = new List<GameObject>();

    private void Update()
    {
        CheckPrefabChanges();
        UpdatePets();
    }

    private void CheckPrefabChanges()
    {
        if (prefabList.Count > petInstances.Count)
        {
            for (int i = petInstances.Count; i < prefabList.Count; i++)
            {
                SpawnPet(prefabList[i]);
            }
        }
        else if (prefabList.Count < petInstances.Count)
        {
            for (int i = petInstances.Count - 1; i >= prefabList.Count; i--)
            {
                DespawnPet(petInstances[i]);
                petInstances.RemoveAt(i);
                petBehaviors.RemoveAt(i);
            }
        }
    }

    private void SpawnPet(GameObject prefab)
    {
        if (prefab == null) return;

        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + player.position;

        GameObject petInstance = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);

        float randomScale = Random.Range(minScale, maxScale);
        petInstance.transform.localScale = Vector3.zero;

        LeanTween.scale(petInstance, Vector3.one * randomScale, spawnAnimationTime)
                 .setEase(LeanTweenType.easeOutBack);

        IPetBehavior behavior = petInstance.GetComponent<IPetBehavior>();
        if (behavior != null)
        {
            GameObject projectileToUse = null;
            float projectileScale = 1f;
            List<Transform> references = null;

            if (petInstance.GetComponent<GuardianControl>() != null)
            {
                projectileToUse = defenderProjectile;
                projectileScale = defenderProjectileScale;
                references = defenderPoints;
            }
            else if (petInstance.GetComponent<AtaquerControl>() != null)
            {
                projectileToUse = attackerProjectile;
                projectileScale = attackerProjectileScale;
                references = attackerPoints;
            }
            else if (petInstance.GetComponent<AssassinControl>() != null)
            {
                projectileToUse = assassinProjectile;
                projectileScale = assassinProjectileScale;
                references = assassinPoints;
            }

            behavior.AssignReferences(player, null, references, enemyLayer, projectileToUse, projectileScale);
            petBehaviors.Add(behavior);
        }

        petInstances.Add(petInstance);
    }

    private void DespawnPet(GameObject pet)
    {
        if (pet == null) return;

        LeanTween.scale(pet, Vector3.zero, despawnAnimationTime)
                 .setEase(LeanTweenType.easeInBack)
                 .setOnComplete(() => Destroy(pet));
    }

    private void UpdatePets()
    {
        foreach (var behavior in petBehaviors)
        {
            if (behavior == null) continue;

            behavior.PerformAction();
        }
    }
}
