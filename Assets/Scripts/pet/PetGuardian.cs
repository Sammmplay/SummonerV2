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
    [SerializeField] private GameObject shieldPrefab;           // Prefab del escudo que se instancia
    [SerializeField] private Transform shieldSpawnPoint;        // Punto desde donde se lanza el escudo
    [SerializeField] private float shieldCooldown = 5f;         // Tiempo entre cada escudo
    [SerializeField] private float detectionRadius = 3f;        // Área en la que detecta enemigos

    [Header("Comportamiento")]
    [SerializeField] private bool enableRodeo = false;          // Si rodea o no al jugador
    [SerializeField] private float rodeoAngleSpeed = 30f;       // Velocidad del rodeo (si está activado)

    [Header("Animación")]
    [SerializeField] private Animator animator;                 // Referencia al Animator de la mascota

    private float cooldownTimer;                                // Temporizador de cooldown del escudo
    private bool escudoActivo = false;                          // Si ya hay un escudo activo o no
    private float currentAngle = 0f;                            // Ángulo actual para el rodeo circular

    /// <summary>
    /// Inicializa referencias y componentes del Guardian.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Actualiza animación de velocidad cada frame.
    /// </summary>
    protected override void Update()
    {
        base.Update();
        animator.SetFloat("Speed", agente.velocity.magnitude);
    }

    /// <summary>
    /// Comportamiento principal del Guardian:
    /// - Lanza el escudo si hay enemigo en rango y el cooldown ha terminado.
    /// - Mira al jugador por defecto, pero mira al enemigo si puede lanzar el escudo.
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

        if (!escudoActivo && enemigoEnRango && cooldownTimer <= 0f && enemigoActual != null)
        {
            // Mira al enemigo solo si puede lanzar escudo
            MirarA(enemigoActual.position);
        }
        else
        {
            // Siempre mira al jugador si no hay cargas disponibles
            MirarA(jugador.position);
        }
    }

    /// <summary>
    /// Calcula el destino deseado de movimiento:
    /// - Si enableRodeo está activo, se mueve en círculo alrededor del jugador.
    /// - Si no, se mantiene cerca del jugador directamente.
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
    /// Detecta el enemigo más cercano dentro del radio de detección.
    /// Guarda su Transform en enemigoActual.
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
    /// Activa el escudo y marca que hay uno activo.
    /// Usa la posición del jugador (etiqueta "Player") para instanciarlo ligeramente elevado.
    /// </summary>
    private void ActivarEscudo()
    {
        if (shieldPrefab != null && shieldSpawnPoint != null)
        {
            GameObject shield = Instantiate(shieldPrefab);
            Transform posPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 posFrontalViewPlayer = new Vector3(posPlayer.position.x, posPlayer.position.y + 1, posPlayer.position.z);
            shield.transform.position = posFrontalViewPlayer;
            escudoActivo = true;
            shield.GetComponent<ShieldB>().OnShieldDestroyed = () => escudoActivo = false;
            animator.SetTrigger("Push");
        }
    }

    /// <summary>
    /// Rota suavemente al Guardian hacia un objetivo dado.
    /// El eje Y se ignora para mantener la rotación en el plano horizontal.
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
    /// Dibuja en el editor el radio de detección de enemigos del Guardian.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
