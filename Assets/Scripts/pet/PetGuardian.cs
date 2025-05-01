using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Mascota guardiana que daña y ralentiza enemigos cercanos, con animación y sonido.
/// </summary>
public class PetGuardian : PetBase
{
    [Header("Aura de contacto")]
    public float attackCooldown = 3f;
    public float slowMultiplier = 0.5f;
    public float slowDuration = 1f;
    public float damage = 1f;

    [Header("Animación y sonido")]
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip pushClip;

    [Header("Control de alcance")]
    public float detectionRadius = 5f;

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

            stats.TakeDamage(damage);
            StartCoroutine(ReducirVelocidadTemporal(stats));

            // ✅ Animación empuje
            if (animator != null)
                animator.SetTrigger("push");

            // ✅ Sonido empuje
            if (audioSource != null && pushClip != null)
                audioSource.PlayOneShot(pushClip);
        }
    }

    private IEnumerator ReducirVelocidadTemporal(EnemyStats enemigo)
    {
        float original = enemigo.Speed;
        enemigo.Speed *= slowMultiplier;

        yield return new WaitForSeconds(slowDuration);

        if (enemigo != null)
            enemigo.Speed = original;
    }

    protected override void ComportamientoPersonalizado()
    {
        // ✅ Detectar movimiento para Idle/Run
        float velocity = agente.velocity.magnitude;
        if (animator != null)
            animator.SetFloat("velocity", velocity);

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
