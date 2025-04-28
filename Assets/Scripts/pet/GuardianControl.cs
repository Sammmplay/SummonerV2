using System.Collections.Generic;
using UnityEngine;

public class GuardianControl : MonoBehaviour, IPetBehavior
{
    private Transform player;
    private List<Transform> enemies;
    private List<Transform> referencePoints;
    private LayerMask enemyLayer;
    private GameObject projectilePrefab;
    private int myIndex;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 projectileOffset = new Vector3(0f, 0f, 1.5f);

    [Header("Proyectil")]
    [SerializeField] private float damagePerSecond = 1f;
    [SerializeField] private float pushForce = 4f;

    private GameObject activeProjectile;

    public void AssignReferences(Transform _player, List<Transform> _enemies, List<Transform> _referencePoints, LayerMask _enemyLayer, GameObject _projectilePrefab)
    {
        player = _player;
        enemies = _enemies;
        referencePoints = _referencePoints;
        enemyLayer = _enemyLayer;
        projectilePrefab = _projectilePrefab;

        myIndex = Mathf.Min(transform.GetSiblingIndex(), referencePoints.Count - 1);

        SpawnProjectile();
    }

    private void SpawnProjectile()
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos = transform.position + transform.TransformDirection(projectileOffset);
        activeProjectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity, transform);

        Collider col = activeProjectile.GetComponent<Collider>();
        if (col != null) col.isTrigger = true;

        GuardianProjectile projScript = activeProjectile.AddComponent<GuardianProjectile>();
        projScript.damagePerSecond = damagePerSecond;
        projScript.pushForce = pushForce;
    }

    public void PerformAction()
    {
        if (referencePoints == null || referencePoints.Count == 0) return;

        Vector3 targetPos = referencePoints[myIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (activeProjectile != null)
        {
            activeProjectile.transform.position = transform.position + transform.TransformDirection(projectileOffset);
        }
    }
}
