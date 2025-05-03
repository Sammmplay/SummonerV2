using UnityEngine;

/// <summary>
/// Mascota atacante que dispara proyectiles hacia el enemigo m�s cercano.
/// Usa Animator para animaci�n y reproduce sonido al impactar.
/// Controla completamente el da�o, direcci�n y velocidad del proyectil.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PetAttacker : PetBase
{
    [Header("Ataque a distancia")]
    [SerializeField] private float attackInterval = 1f; // Intervalo entre disparos
    [SerializeField] private float damage = 1f; // Da�o por proyectil
    [SerializeField] private float projectileSpeed = 10f; // Velocidad del proyectil
    [SerializeField] private float projectileLifetime = 5f; // Tiempo de vida del proyectil
    [SerializeField] private Transform shootOrigin; // Punto de origen del disparo

    [Header("Audio y Animaci�n")]
    [SerializeField] private AudioClip impactoSonido; // Sonido de impacto al golpear enemigo
    [SerializeField] private Animator animator; // Controlador de animaciones

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
    /// Define el comportamiento de ataque:
    /// rota hacia el enemigo, dispara a intervalos definidos
    /// y controla animaci�n de disparo.
    /// </summary>
    protected override void ComportamientoPersonalizado()
    {
        if (enemigoActual == null)
        {
            transform.LookAt(jugador);
            animator.SetFloat("Speed", agente.velocity.magnitude);
            return;
        }

        // Rotaci�n horizontal hacia el enemigo
        Vector3 lookDir = enemigoActual.position - transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 10f);

        cooldown += Time.deltaTime;
        animator.SetFloat("Speed", agente.velocity.magnitude);

        // Disparar si el cooldown lo permite
        if (cooldown >= attackInterval)
        {
            cooldown = 0f;
            Disparar(enemigoActual.position);
            animator.SetTrigger("Shoot");
        }
    }

    /// <summary>
    /// Instancia y lanza un proyectil hacia el objetivo especificado.
    /// Configura direcci�n, velocidad, da�o y sonido.
    /// </summary>
    /// <param name="objetivo">Posici�n del enemigo objetivo.</param>
    private void Disparar(Vector3 objetivo)
    {
        if (proyectilBase == null || shootOrigin == null) return;

        Vector3 dir = objetivo - shootOrigin.position;
        dir.y = 0;
        dir.Normalize();

        GameObject proyectil = Instantiate(proyectilBase, shootOrigin.position, Quaternion.identity);
        var handler = proyectil.AddComponent<ProjectileHandler>();
        handler.Initialize(dir, projectileSpeed, damage, EnemyLayer, projectileLifetime, impactoSonido, audioSource);
    }

    /// <summary>
    /// Clase interna que maneja el comportamiento de cada proyectil.
    /// Se encarga del movimiento, colisi�n y aplicaci�n de da�o.
    /// </summary>
    private class ProjectileHandler : MonoBehaviour
    {
        private Vector3 direction;
        private float speed, damage;
        private LayerMask enemyLayer;
        private AudioClip impactoSonido;
        private AudioSource audioSource;

        /// <summary>
        /// Inicializa el proyectil con los par�metros necesarios.
        /// </summary>
        /// <param name="dir">Direcci�n de movimiento.</param>
        /// <param name="spd">Velocidad del proyectil.</param>
        /// <param name="dmg">Da�o que aplica al impactar.</param>
        /// <param name="layer">Capa de los enemigos.</param>
        /// <param name="lifetime">Tiempo de vida antes de autodestrucci�n.</param>
        /// <param name="sonido">Clip de audio a reproducir al impactar.</param>
        /// <param name="source">AudioSource de la mascota para reproducir sonido.</param>
        public void Initialize(Vector3 dir, float spd, float dmg, LayerMask layer, float lifetime, AudioClip sonido, AudioSource source)
        {
            direction = dir.normalized;
            speed = spd;
            damage = dmg;
            enemyLayer = layer;
            impactoSonido = sonido;
            audioSource = source;
            Destroy(gameObject, lifetime);
        }

        /// <summary>
        /// Mueve el proyectil en la direcci�n asignada cada frame.
        /// </summary>
        private void Update()
        {
            transform.position += direction * speed * Time.deltaTime;
        }

        /// <summary>
        /// Maneja la colisi�n con enemigos:
        /// aplica da�o y reproduce sonido.
        /// </summary>
        /// <param name="other">Collider con el que colisiona.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & enemyLayer.value) == 0) return;

            var stats = other.GetComponent<EnemyStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
                if (impactoSonido != null && audioSource != null)
                    audioSource.PlayOneShot(impactoSonido);
            }

            Destroy(gameObject);
        }
    }
}
