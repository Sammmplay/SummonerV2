using System.Collections.Generic;
using UnityEngine;

public class AssassinControl : MonoBehaviour, IPetBehavior
{
    private Transform player;
    private List<Transform> enemies;
    private List<Transform> referencePoints;
    private LayerMask enemyLayer;
    private GameObject projectilePrefab;
    private int myIndex;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Proyectiles")]
    [SerializeField] private int maxProjectiles = 5;
    [SerializeField] private float orbitRadius = 1.5f;
    [SerializeField] private float orbitSpeed = 180f;

    [Header("Ataque")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private float projectileDamage = 0.5f;
    [SerializeField] private float projectileForce = 15f;

    private List<GameObject> orbitingProjectiles = new List<GameObject>();
    private float cooldownTimer;
    private bool isAttacking = false;
    private Transform targetEnemy;

    public void AssignReferences(Transform _player, List<Transform> _enemies, List<Transform> _referencePoints, LayerMask _enemyLayer, GameObject _projectilePrefab)
    {
        player = _player;
        enemies = _enemies;
        referencePoints = _referencePoints;
        enemyLayer = _enemyLayer;
        projectilePrefab = _projectilePrefab;

        myIndex = Mathf.Min(transform.GetSiblingIndex(), referencePoints.Count - 1);
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
        RotateProjectiles();
    }

    public void PerformAction()
    {
        if (isAttacking || referencePoints == null || referencePoints.Count == 0) return;

        Vector3 targetPos = referencePoints[myIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (orbitingProjectiles.Count < maxProjectiles)
        {
            TrySpawnProjectile();
        }
        else
        {
            TryAttack();
        }
    }

    private void TrySpawnProjectile()
    {
        if (projectilePrefab == null) return;

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.transform.SetParent(transform);

        AssassinProjectile projScript = proj.AddComponent<AssassinProjectile>();
        projScript.damage = projectileDamage;

        orbitingProjectiles.Add(proj);
    }

    private void RotateProjectiles()
    {
        if (orbitingProjectiles.Count == 0) return;

        float angleStep = 360f / orbitingProjectiles.Count;
        for (int i = 0; i < orbitingProjectiles.Count; i++)
        {
            if (orbitingProjectiles[i] == null) continue;

            float angle = angleStep * i + Time.time * orbitSpeed;
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad)) * orbitRadius;
            orbitingProjectiles[i].transform.localPosition = offset;
        }
    }

    private void TryAttack()
    {
        Collider[] foundEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (foundEnemies.Length > 0 && cooldownTimer <= 0f)
        {
            targetEnemy = foundEnemies[0].transform;
            StartCoroutine(AttackSequence());
        }
    }

    private System.Collections.IEnumerator AttackSequence()
    {
        isAttacking = true;

        foreach (GameObject proj in orbitingProjectiles)
        {
            if (proj == null) continue;

            proj.transform.SetParent(null);

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null && targetEnemy != null)
            {
                Vector3 dir = (targetEnemy.position - proj.transform.position).normalized;
                rb.AddForce(dir * projectileForce, ForceMode.Impulse);
            }
        }

        orbitingProjectiles.Clear();

        yield return new WaitForSeconds(1.5f);

        cooldownTimer = attackCooldown;
        isAttacking = false;
    }
}
