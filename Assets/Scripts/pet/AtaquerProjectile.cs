using UnityEngine;

public class AtaquerProjectile : MonoBehaviour
{
    public float damage = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStats enemy = other.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject); // Se destruye tras impactar
        }
    }
}
