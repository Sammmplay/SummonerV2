using UnityEngine;

/// <summary>
/// PetGuardian: mascota defensiva que sigue al jugador, mantiene distancia,
/// mira al jugador por defecto y al enemigo solo cuando puede lanzar el escudo.
/// Usa Animator (Speed, Push) y tiene opción de rodeo.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PetGuardian : PetBase
{
    [Header("Escudo")]
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private Transform shieldSpawnPoint;
    [SerializeField] private float shieldCooldown = 5f;
    [SerializeField] private float detectionRadius = 3f;

    [Header("Comportamiento")]
    [SerializeField] private bool enableRodeo = false;
    [SerializeField] private float rodeoAngleSpeed = 30f;

    [Header("Animación")]
    [SerializeField] private Animator animator;

    private float cooldownTimer;
    private bool escudoActivo = false;
    private float currentAngle = 0f;

    /// <summary>
    /// Inicializa componentes.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Actualiza lógica de animación.
    /// </summary>
    protected override void Update()
    {
        base.Update();
        animator.SetFloat("Speed", agente.velocity.magnitude);
    }

    /// <summary>
    /// Comportamiento principal del Guardian.
    /// </summary>
    protected override void ComportamientoPersonalizado()
    {
        cooldownTimer -= Time.deltaTime;

        bool enemigoEnRango = EnemigoEnRango();

        if (!escudoActivo && enemigoEnRango && cooldownTimer <= 0f)
        {
            ActivarEscudo();
            cooldownTimer = shieldCooldown;
        }

        // Decide hacia dónde mirar
        if (!escudoActivo && enemigoEnRango && cooldownTimer <= 0f && enemigoActual != null)
        {
            // Mira al enemigo solo si puede lanzar escudo
            MirarA(enemigoActual.position);
        }
        else
        {
            // Siempre mira al jugador si no hay cargas
            MirarA(jugador.position);
        }
    }

    /// <summary>
    /// Calcula destino: jugador + opcional rodeo.
    /// </summary>
    protected override Vector3 CalcularDestino()
    {
        if (jugador == null)
            return transform.position;

        if (enableRodeo)
        {
            currentAngle += rodeoAngleSpeed * Time.deltaTime;
            if (currentAngle >= 360f) currentAngle -= 360f;

            Vector3 offset = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distanciaAlJugador;
            return jugador.position + offset;
        }

        return jugador.position;
    }

    /// <summary>
    /// Comprueba si hay enemigos cerca.
    /// </summary>
    private bool EnemigoEnRango()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, EnemyLayer);
        float distanciaMin = Mathf.Infinity;
        enemigoActual = null;

        foreach (var col in hits)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < distanciaMin)
            {
                distanciaMin = dist;
                enemigoActual = col.transform;
            }
        }

        return enemigoActual != null;
    }

    /// <summary>
    /// Activa escudo y marca como activo.
    /// </summary>
    private void ActivarEscudo()
    {
        if (shieldPrefab != null && shieldSpawnPoint != null)
        {
            GameObject shield = Instantiate(shieldPrefab, shieldSpawnPoint.position, transform.rotation);
            escudoActivo = true;
            shield.GetComponent<ShieldB>().OnShieldDestroyed = () => escudoActivo = false;
            animator.SetTrigger("Push");
        }
    }

    /// <summary>
    /// Hace que el Guardian mire suavemente a un objetivo.
    /// </summary>
    private void MirarA(Vector3 objetivo)
    {
        Vector3 lookDir = objetivo - transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    /// <summary>
    /// Dibuja el radio de detección en el editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
