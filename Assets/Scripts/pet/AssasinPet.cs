using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Mascota tipo Assassin que atraviesa enemigos en cadena,
/// aplicando da�o, mostrando un �rea de detecci�n editable
/// y alej�ndose tras impactar para evitar atascos.
/// </summary>
[RequireComponent(typeof(Collider), typeof(AudioSource), typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PetAssassin : PetBase
{
    [Header("Par�metros de Embestida")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float esperaEntreCadenas = 5f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private int maxRebotes = 3;
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private float pushBackDistance = 1f; // distancia para alejarse tras golpear

    [Header("Gizmo Visual")]
    [SerializeField] private Color gizmoColor = new Color(1f, 0f, 0f, 0.3f); // color editable en el editor

    [Header("Audio y Animaci�n")]
    [SerializeField] private AudioClip impactoSonido;
    [SerializeField] private Animator animator;

    private bool isDashing = false;
    private float cooldown = 0f;
    private int rebotesRestantes;
    private AudioSource audioSource;
    private Collider petCollider;
    private Rigidbody rb;
    private List<Transform> enemigosGolpeados = new();

    /// <summary>
    /// Inicializa componentes y valida referencias.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        petCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        petCollider.isTrigger = true;
        rb.isKinematic = true;

        if (animator == null)
            animator = GetComponent<Animator>();

        rebotesRestantes = maxRebotes;
    }

    /// <summary>
    /// Actualiza las animaciones y el estado cada frame.
    /// </summary>
    protected override void Update()
    {
        base.Update();
        animator.SetFloat("Speed", agente.velocity.magnitude);
        animator.SetBool("IsDashing", isDashing);
    }

    /// <summary>
    /// Controla el comportamiento espec�fico del Assassin:
    /// seguir al jugador o iniciar el dash.
    /// </summary>
    protected override void ComportamientoPersonalizado()
    {
        if (isDashing)
        {
            RealizarDash();
            return;
        }

        cooldown += Time.deltaTime;

        if (enemigoActual != null && cooldown >= esperaEntreCadenas)
        {
            IniciarDash();
        }
    }

    /// <summary>
    /// Inicia la secuencia de embestidas.
    /// </summary>
    private void IniciarDash()
    {
        isDashing = true;
        cooldown = 0f;
        rebotesRestantes = maxRebotes;
        enemigosGolpeados.Clear();
        agente.updatePosition = false;
    }

    /// <summary>
    /// Realiza el movimiento de dash hacia el enemigo actual.
    /// </summary>
    private void RealizarDash()
    {
        if (enemigoActual == null || rebotesRestantes <= 0)
        {
            TerminarDash();
            return;
        }

        Vector3 direccion = (enemigoActual.position - transform.position).normalized;
        transform.position += direccion * dashSpeed * Time.deltaTime;
        transform.LookAt(enemigoActual);
    }

    /// <summary>
    /// Detecta colisiones con enemigos durante el dash.
    /// Aplica da�o y busca el siguiente objetivo.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (!isDashing) return;
        if (((1 << other.gameObject.layer) & EnemyLayer.value) == 0) return;

        Transform enemigo = other.transform;
        if (enemigosGolpeados.Contains(enemigo)) return;

        var stats = enemigo.GetComponent<EnemiesController>();
        if (stats != null)
        {
            stats.TakeDamage(damage);
            if (impactoSonido != null)
                audioSource.PlayOneShot(impactoSonido);
        }

        enemigosGolpeados.Add(enemigo);
        AplicarEmpujeAtras(enemigo);

        rebotesRestantes--;

        if (rebotesRestantes > 0)
        {
            BuscarSiguienteEnemigo();
            if (enemigoActual == null)
                TerminarDash();
        }
        else
        {
            TerminarDash();
        }
    }

    /// <summary>
    /// Aplica un empuje hacia atr�s para evitar quedarse atrapado.
    /// </summary>
    /// <param name="enemigo">El enemigo golpeado.</param>
    private void AplicarEmpujeAtras(Transform enemigo)
    {
        Vector3 awayFromEnemy = (transform.position - enemigo.position).normalized;
        transform.position += awayFromEnemy * pushBackDistance;
    }

    /// <summary>
    /// Busca el siguiente enemigo dentro del radio de detecci�n.
    /// </summary>
    private void BuscarSiguienteEnemigo()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, detectionRadius, EnemyLayer);
        float minDist = Mathf.Infinity;
        Transform siguiente = null;

        foreach (var col in cols)
        {
            if (enemigosGolpeados.Contains(col.transform)) continue;

            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                siguiente = col.transform;
            }
        }

        enemigoActual = siguiente;
    }

    /// <summary>
    /// Finaliza la secuencia de dash y regresa al estado normal.
    /// </summary>
    private void TerminarDash()
    {
        isDashing = false;
        agente.updatePosition = true;
        enemigoActual = null;
    }

    /// <summary>
    /// Calcula el destino actual del Assassin (jugador o enemigo).
    /// </summary>
    /// <returns>Posici�n objetivo.</returns>
    protected override Vector3 CalcularDestino()
    {
        if (!isDashing && jugador != null)
            return jugador.position;

        if (isDashing && enemigoActual != null)
            return enemigoActual.position;

        return jugador != null ? jugador.position : transform.position;
    }

    /// <summary>
    /// Dibuja el radio de detecci�n en el editor con color configurable.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Color gizmoTransparent = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, gizmoColor.a);
        Gizmos.color = gizmoTransparent;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
