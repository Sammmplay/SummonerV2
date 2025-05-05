using UnityEngine;

/// <summary>
/// Mascota atacante que sigue al jugador, apunta al enemigo y dispara proyectiles usando Animator.
/// </summary>
[RequireComponent(typeof(Animator))]
public class ataquerPet : PetBase
{
    [Header("Ataque a distancia")]
    [SerializeField] private float attackInterval = 1f;     // Tiempo entre disparos
    [SerializeField] private Transform shootOrigin;         // Punto de spawn del proyectil
    [SerializeField] private Animator animator;            // Animator con Speed y Shoot

    private float cooldown;

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
    /// Actualiza las animaciones de movimiento.
    /// </summary>
    protected override void Update()
    {
        base.Update();

        // 🔥 Calcula velocidad para Speed en Animator
        float agentSpeed = agente.velocity.magnitude;
        animator.SetFloat("Speed", agentSpeed);
    }

    /// <summary>
    /// Apunta al enemigo y dispara si está cerca, sin dejar de seguir al jugador.
    /// </summary>
    protected override void ComportamientoPersonalizado()
    {
        if (enemigoActual != null)
        {
            // 🔥 Rota hacia el enemigo (solo mira, no camina hacia él)
            Vector3 lookDir = enemigoActual.position - transform.position;
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 10f);

            // 🔥 Maneja cooldown y disparo
            cooldown += Time.deltaTime;
            if (cooldown >= attackInterval)
            {
                cooldown = 0f;
                Disparar();
                animator.SetTrigger("Shoot");
            }
        }
    }

    /// <summary>
    /// Instancia un proyectil y lo orienta hacia el enemigo.
    /// </summary>
    private void Disparar()
    {
        if (proyectilBase == null || shootOrigin == null || enemigoActual == null) return;

        GameObject proyectil = Instantiate(proyectilBase, shootOrigin.position, Quaternion.identity);
        proyectil.transform.forward = (enemigoActual.position - shootOrigin.position).normalized;
    }

    /// <summary>
    /// Siempre sigue al jugador como destino.
    /// </summary>
    protected override Vector3 CalcularDestino()
    {
        return jugador != null ? jugador.position : transform.position;
    }
}
