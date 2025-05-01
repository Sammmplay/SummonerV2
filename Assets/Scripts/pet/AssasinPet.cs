using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Mascota asesina que embiste a enemigos en cadena, con animación y sonido.
/// </summary>
public class PetAssassin : PetBase
{
    [Header("Embestidas")]
    [SerializeField] public float dashSpeed = 15f;
    [SerializeField] public float reboteDelay = 0.15f;
    [SerializeField] public float damage = 1f;
    [SerializeField] public float esperaEntreCadena = 1f;

    [Header("Animación y sonido")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dashClip;

    private bool enAtaque = false;
    private float cooldown = 0f;

    protected override void ComportamientoPersonalizado()
    {
        if (enAtaque) return;

        cooldown += Time.deltaTime;

        if (enemigos != null && enemigos.Count > 0 && cooldown >= esperaEntreCadena)
        {
            StartCoroutine(RealizarEmbestidas());
            return;
        }

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

            // ✅ Lanzar animación de dash
            if (animator != null)
                animator.SetTrigger("Dash");

            // ✅ Reproducir sonido de dash
            if (audioSource != null && dashClip != null)
                audioSource.PlayOneShot(dashClip);

            while (tiempo < duracion)
            {
                tiempo += Time.deltaTime;
                transform.position = Vector3.Lerp(inicio, destino, tiempo / duracion);
                yield return null;
            }

            transform.position = destino;

            var stats = enemigo.GetComponent<EnemyStats>();
            if (stats != null)
                stats.TakeDamage(damage);

            yield return new WaitForSeconds(reboteDelay);
        }

        agente.enabled = true;
        enAtaque = false;
    }
}
