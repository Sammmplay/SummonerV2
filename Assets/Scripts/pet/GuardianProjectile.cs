using UnityEngine;

public class GuardianProjectile : MonoBehaviour
{
    public float damagePerSecond = 1f;
    public float pushForce = 4f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStats enemy = other.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(damagePerSecond * Time.deltaTime);
            }

            Vector3 pushDir = (other.transform.position - transform.position).normalized;
            other.transform.position += pushDir * pushForce * Time.deltaTime;
        }
    }
}
