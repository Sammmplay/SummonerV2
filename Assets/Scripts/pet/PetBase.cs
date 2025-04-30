// PetBase.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

/// <summary>
/// Clase base abstracta para todas las mascotas.
/// Gestiona el seguimiento al jugador, par�metros de navegaci�n y
/// expone la lista de enemigos proporcionada por el PetManager.
/// Las subclases implementan su propio comportamiento en ComportamientoPersonalizado().
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public abstract class PetBase : MonoBehaviour
{
    [Header("Par�metros de Seguimiento")]
    [Tooltip("Distancia m�nima que mantendr� respecto al jugador.")]
    [SerializeField] protected float distanciaAlJugador = 2f;

    [Header("Par�metros de Navegaci�n")]
    [SerializeField] protected float velocidad = 3.5f;
    [SerializeField] protected float aceleracion = 8f;
    [SerializeField] protected float giro = 120f;

    /// <summary>
    /// Capa de enemigos para detecci�n, asignada desde el PetManager.
    /// </summary>
    [HideInInspector] public LayerMask EnemyLayer;

    // Referencias inyectadas por el PetManager
    protected Transform jugador;
    protected GameObject proyectilBase;
    protected List<Transform> enemigos;
    protected PetManager petManager;

    // Componente de navegaci�n
    protected NavMeshAgent agente;

    /// <summary>
    /// Configura referencias globales: jugador, prefab de proyectil,
    /// lista de enemigos y el propio PetManager.
    /// </summary>
    public virtual void Configurar(
        Transform jugadorRef,
        GameObject proyectilRef,
        List<Transform> enemigosRef,
        PetManager managerRef = null)
    {
        jugador = jugadorRef;
        proyectilBase = proyectilRef;
        enemigos = enemigosRef;
        petManager = managerRef;
    }

    /// <summary>
    /// Inicializa y valida el NavMeshAgent; setea par�metros de movimiento.
    /// </summary>
    protected virtual void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        if (agente == null)
        {
            Debug.LogError($"{name} requiere un NavMeshAgent.");
            enabled = false;
            return;
        }
        if (!agente.isOnNavMesh)
        {
            Debug.LogWarning($"{name} instanciado fuera del NavMesh.");
            enabled = false;
            return;
        }

        agente.stoppingDistance = distanciaAlJugador;
        agente.autoBraking = true;
        agente.speed = velocidad;
        agente.acceleration = aceleracion;
        agente.angularSpeed = giro;

        // Opcional: prioridad de evitaci�n aleatoria para evitar amontonamiento
        agente.avoidancePriority = Random.Range(0, 100);
    }

    /// <summary>
    /// Cada frame mueve al agente hacia el destino calculado
    /// y ejecuta el comportamiento espec�fico de la subclase.
    /// </summary>
    protected virtual void Update()
    {
        if (jugador == null || agente == null) return;

        agente.SetDestination(CalcularDestino());
        ComportamientoPersonalizado();
    }

    /// <summary>
    /// Determina a d�nde moverse; por defecto, sigue al jugador.
    /// Las subclases pueden overridear para priorizar enemigos.
    /// </summary>
    protected virtual Vector3 CalcularDestino()
    {
        return jugador.position;
    }

    /// <summary>
    /// M�todo abstracto que cada subclase debe implementar
    /// para definir su comportamiento �nico (ataque, dash, empuje�).
    /// </summary>
    protected abstract void ComportamientoPersonalizado();
}
