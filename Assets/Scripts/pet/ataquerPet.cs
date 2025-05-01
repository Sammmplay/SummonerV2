using UnityEngine;

/// <summary>
/// Mascota atacante que dispara proyectiles en línea recta, con animación y sonido.
/// </summary>
public class PetAttacker : PetBase
{
    [Header("Ataque a distancia")]
    public float attackInterval = 1f;
    public float damage = 1f;
    public float projectileSpeed = 10f;
    public float projectileLifetime = 5f;
    public Transform shootOrigin;

    [Header("Animación y sonido")]
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip shootClip;

    private float cooldown;

    protected override void ComportamientoPersonalizado()
    {
        // Calcula velocidad para animaciones
        float velocity = agente.velocity.magnitude;
        if (animator != null)
            animator.SetFloat("velocity", velocity);

        if (enemigos == null || enemigos.Count == 0)
        {
            transform.LookAt(jugador);
            return;
        }

        Transform target = ObtenerEnemigoMasCercano();
        if (target == null)
        {
            transform.LookAt(jugador);
            return;
        }

        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 10f);

        cooldown += Time.deltaTime;
        if (cooldown >= attackInterval)
        {
            cooldown = 0f;
            Disparar(target.position);

            // ✅ Animación disparo
            if (animator != null)
                animator.SetTrigger("shoot");

            // ✅ Sonido disparo
            if (audioSource != null && shootClip != null)
                audioSource.PlayOneShot(shootClip);
        }
    }

    private Transform ObtenerEnemigoMasCercano()
    {
        Transform closest = null;
        float minDist = float.MaxValue;

        foreach (Transform e in enemigos)
        {
            float d = Vector3.Distance(transform.position, e.position);
            if (d < minDist)
            {
                minDist = d;
                closest = e;
            }
        }

        return closest;
    }

    private void Disparar(Vector3 objetivo)
    {
        if (proyectilBase == null || shootOrigin == null) return;

        Vector3 dir = objetivo - shootOrigin.position;
        dir.y = 0;
        dir.Normalize();

        GameObject proyectil = Instantiate(proyectilBase, shootOrigin.position, Quaternion.identity);
        var handler = proyectil.AddComponent<ProjectileHandler>();
        handler.Initialize(dir, projectileSpeed, damage, EnemyLayer, projectileLifetime);
    }

    private class ProjectileHandler : MonoBehaviour
    {
        private Vector3 direction;
        private float speed, damage;
        private LayerMask enemyLayer;

        public void Initialize(Vector3 dir, float spd, float dmg, LayerMask layer, float lifetime)
        {
            direction = dir.normalized;
            speed = spd;
            damage = dmg;
            enemyLayer = layer;
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.position += direction * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & enemyLayer) == 0) return;

            var stats = other.GetComponent<EnemyStats>();
            if (stats != null) stats.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
