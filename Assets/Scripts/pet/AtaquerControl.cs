using System.Collections.Generic;
using UnityEngine;

public class AtaquerControl : MonoBehaviour, IPetBehavior
{
    private Transform player;
    private List<Transform> enemies;
    private List<Transform> referencePoints;
    private LayerMask enemyLayer;
    private GameObject projectilePrefab;
    private int myIndex;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Ataque")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float projectileForce = 15f;
    [SerializeField] private float projectileDamage = 2f;
    [SerializeField] private float attackRange = 10f;

    private float cooldownTimer;

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
    }

    public void PerformAction()
    {
        if (referencePoints == null || referencePoints.Count == 0) return;

        Vector3 targetPos = referencePoints[myIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        TryAttack();
    }

    private void TryAttack()
    {
        if (cooldownTimer > 0f || projectilePrefab == null) return;

        Collider[] foundEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (foundEnemies.Length == 0) return;

        Transform fastestEnemy = SelectFastestEnemy(foundEnemies);
        if (fastestEnemy != null)
        {
            ShootProjectile(fastestEnemy);
            cooldownTimer = attackCooldown;
        }
    }

    private Transform SelectFastestEnemy(Collider[] enemies)
    {
        float highestSpeed = -1f;
        Transform target = null;

        foreach (Collider col in enemies)
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            if (enemy != null && enemy.Speed > highestSpeed)
            {
                highestSpeed = enemy.Speed;
                target = enemy.transform;
            }
        }

        return target;
    }

    private void ShootProjectile(Transform target)
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        AtaquerProjectile projScript = proj.AddComponent<AtaquerProjectile>();
        projScript.damage = projectileDamage;

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            rb.AddForce(dir * projectileForce, ForceMode.Impulse);
        }
    }
}
