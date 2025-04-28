using UnityEngine;

public class AssassinProjectile : MonoBehaviour
{
    public float damage = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStats enemy = other.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject); // El proyectil desaparece al impactar
        }
    }
}

