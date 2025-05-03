using UnityEngine;

/// <summary>
/// Mascota tipo Defender que permanece cerca del jugador (máx. 1.5 m),
/// detecta enemigos cercanos usando el sistema del PetBase
/// y los empuja para proteger al jugador.
/// Usa Animator para animaciones y AudioSource para sonido.
/// </summary>
[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class PetDefender : PetBase
{
    [Header("Defensa")]
    [SerializeField] private float empujeIntervalo = 3f; // Tiempo entre empujes
    [SerializeField] private float empujeFuerza = 1f;    // Daño aplicado al empujar
    [SerializeField] private float distanciaMaximaJugador = 1.5f; // ~5 pies

    [Header("Audio y Animación")]
    [SerializeField] private AudioClip impactoSonido; // Sonido al impactar enemigo
    [SerializeField] private Animator animator;       // Animator de la pet

    private float cooldown;
    private AudioSource audioSource;

    /// <summary>
    /// Inicializa componentes (Animator y AudioSource).
    /// </summary>
    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Controla el comportamiento principal:
    /// mantener distancia al jugador, buscar enemigo más cercano
    /// y empujarlo si está disponible.
    /// </summary>
    protected override void ComportamientoPersonalizado()
    {
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        // Si se aleja demasiado, regresar al jugador
        if (distanciaAlJugador > distanciaMaximaJugador)
        {
            agente.SetDestination(jugador.position);
            animator.SetFloat("Speed", agente.velocity.magnitude);
            return;
        }

        if (enemigoActual != null)
        {
            agente.SetDestination(enemigoActual.position);
            transform.LookAt(enemigoActual);
            cooldown += Time.deltaTime;
            animator.SetFloat("Speed", agente.velocity.magnitude);

            if (cooldown >= empujeIntervalo)
            {
                cooldown = 0f;
                EmpujarEnemigo(enemigoActual);
                animator.SetTrigger("Push");
            }
        }
        else
        {
            // Si no hay enemigos, mantenerse cerca del jugador
            agente.SetDestination(jugador.position);
            animator.SetFloat("Speed", agente.velocity.magnitude);
        }
    }

    /// <summary>
    /// Aplica daño al enemigo y lo empuja ligeramente hacia afuera.
    /// </summary>
    /// <param name="enemigo">Enemigo objetivo.</param>
    private void EmpujarEnemigo(Transform enemigo)
    {
        var stats = enemigo.GetComponent<EnemyStats>();
        if (stats != null)
        {
            stats.TakeDamage(empujeFuerza);
            if (impactoSonido != null)
                audioSource.PlayOneShot(impactoSonido);
        }

        // Empuje ligero hacia afuera, solo en posición (sin física)
        Vector3 awayFromPlayer = (enemigo.position - jugador.position).normalized;
        enemigo.position += awayFromPlayer * 0.5f;
    }
}
