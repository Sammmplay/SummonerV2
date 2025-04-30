using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Mascota asesina que embiste a enemigos en cadena.
/// Alterna entre ataque y seguir al jugador.
/// </summary>
public class PetAssassin : PetBase
{
    [Header("Embestidas")]
    [Tooltip("Velocidad del dash entre enemigos.")]
    [SerializeField] private float dashSpeed = 15f;

    [Tooltip("Tiempo entre cada rebote (s).")]
    [SerializeField] private float reboteDelay = 0.15f;

    [Tooltip("Tiempo de espera entre cadenas (s).")]
    [SerializeField] private float esperaEntreCadena = 2f;

    [Tooltip("Daño que aplica por embestida.")]
    [SerializeField] private float damage = 1f;

    private bool enAtaque = false;
    private float cooldown = 0f;

    protected override void ComportamientoPersonalizado()
    {
        // Si está atacando, no seguir al jugador
        if (enAtaque) return;

        // Cooldown para próximo ataque
        cooldown += Time.deltaTime;

        if (enemigos != null && enemigos.Count > 0 && cooldown >= esperaEntreCadena)
        {
            StartCoroutine(RealizarEmbestidas());
            return;
        }

        // Si no hay enemigos o está en cooldown, seguir al jugador
        if (agente.enabled)
        {
            agente.SetDestination(jugador.position);
            transform.LookAt(jugador);
        }
    }

    private IEnumerator RealizarEmbestidas()
    {
        enAtaque = true;
        cooldown = 0f;
        agente.enabled = false;

        List<Transform> enemigosOrdenados = new List<Transform>(enemigos);
        enemigosOrdenados.Sort((a, b) =>
            Vector3.Distance(transform.position, a.position)
            .CompareTo(Vector3.Distance(transform.position, b.position)));

        foreach (Transform enemigo in enemigosOrdenados)
        {
            if (enemigo == null) continue;

            Vector3 inicio = transform.position;
            Vector3 destino = enemigo.position;

            float duracion = Vector3.Distance(inicio, destino) / dashSpeed;
            float tiempo = 0f;

            while (tiempo < duracion)
            {
                tiempo += Time.deltaTime;
                transform.position = Vector3.Lerp(inicio, destino, tiempo / duracion);
                yield return null;
            }

            transform.position = destino;

            // Aplicar daño
            var stats = enemigo.GetComponent<EnemyStats>();
            if (stats != null)
                stats.TakeDamage(damage);

            // Pequeño delay entre embestidas
            yield return new WaitForSeconds(reboteDelay);
        }

        // Reactivar seguimiento al jugador
        agente.enabled = true;
        enAtaque = false;
    }
}
