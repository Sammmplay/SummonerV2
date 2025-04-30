using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Mascota guardiana que daña y ralentiza enemigos cercanos sin alejarse demasiado del jugador.
/// </summary>
public class PetGuardian : PetBase
{
    [Header("Aura de contacto")]
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float slowMultiplier = 0.5f;
    [SerializeField] private float slowDuration = 1f;

    [Header("Control de alcance")]
    [Tooltip("Distancia máxima que puede alejarse del jugador.")]
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

            stats.TakeDamage(1f);
            StartCoroutine(ReducirVelocidadTemporal(stats));
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
