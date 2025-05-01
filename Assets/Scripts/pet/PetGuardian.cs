using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Mascota guardiana que daña y ralentiza enemigos cercanos por contacto,
/// con animación y sonido.
/// Usa un cooldown por enemigo individual.
/// </summary>
public class PetGuardian : PetBase
{
    [Header("Aura de contacto")]
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float slowMultiplier = 0.5f;
    [SerializeField] private float slowDuration = 1f;

    [Header("Daño")]
    [Tooltip("Daño aplicado por contacto (modificable por PowerUps).")]
    [SerializeField] public float damage = 1f;

    [Header("Animación y sonido")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pushClip;

    [Header("Control de alcance")]
    [SerializeField] private float detectionRadius = 5f;

    private Dictionary<Transform, float> cooldowns = new();

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & EnemyLayer) == 0) return;

        Transform enemigo = other.transform;
        var stats = enemigo.GetComponent<EnemyStats>();
        if (stats == null) return;

        if (!cooldowns.ContainsKey(enemigo)) cooldowns[enemigo] = -attackCooldown;

        if (Time.time - cooldowns[enemigo] >= attackCooldown)
        {
            cooldowns[enemigo] = Time.time;

            // ✅ Daño configurable
            stats.TakeDamage(damage);
            StartCoroutine(ReducirVelocidadTemporal(stats));

            // ✅ Animación de empuje
            if (animator != null)
                animator.SetTrigger("Push");

            // ✅ Sonido de empuje
            if (audioSource != null && pushClip != null)
                audioSource.PlayOneShot(pushClip);
        }
    }

    /// <summary>
    /// Reduce temporalmente la velocidad del enemigo.
    /// </summary>
    private IEnumerator ReducirVelocidadTemporal(EnemyStats enemigo)
    {
        float original = enemigo.Speed;
        enemigo.Speed *= slowMultiplier;

        yield return new WaitForSeconds(slowDuration);

        if (enemigo != null)
            enemigo.Speed = original;
    }

    /// <summary>
    /// Ejecuta el comportamiento principal:
    /// buscar al enemigo más cercano dentro del rango
    /// y moverse hacia él, si no, seguir al jugador.
    /// </summary>
    protected override void ComportamientoPersonalizado()
    {
        Transform enemigo = ObtenerEnemigoMasCercanoEnRango();

        if (enemigo != null)
        {
            float distEnemigo = Vector3.Distance(jugador.position, enemigo.position);

            if (distEnemigo <= detectionRadius)
                agente.SetDestination(enemigo.position);
            else
                agente.SetDestination(jugador.position);

            transform.LookAt(enemigo);
        }
        else
        {
            agente.SetDestination(jugador.position);
            transform.LookAt(jugador);
        }
    }

    /// <summary>
    /// Devuelve el enemigo más cercano dentro del rango de detección.
    /// </summary>
    private Transform ObtenerEnemigoMasCercanoEnRango()
    {
        Transform cercano = null;
        float minDist = float.MaxValue;

        foreach (var e in enemigos)
        {
            float dist = Vector3.Distance(jugador.position, e.position);
            if (dist <= detectionRadius && dist < minDist)
            {
                minDist = dist;
                cercano = e;
            }
        }

        return cercano;
    }
}
