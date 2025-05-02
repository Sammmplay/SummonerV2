using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Clase base para mascotas. Gestiona movimiento, detección y comportamiento.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public abstract class PetBase : MonoBehaviour
{
    [Header("Parámetros")]
    [SerializeField] protected float distanciaAlJugador = 2f;
    [SerializeField] protected float rangoDeteccion = 5f;

    protected Transform jugador;
    protected GameObject proyectilBase;
    protected PetManager petManager;
    protected NavMeshAgent agente;

    public LayerMask EnemyLayer { get; set; }
    protected Transform enemigoActual;

    public virtual void Configurar(Transform jugadorRef, GameObject proyectilRef, PetManager managerRef)
    {
        jugador = jugadorRef;
        proyectilBase = proyectilRef;
        petManager = managerRef;

        if (petManager != null)
            EnemyLayer = petManager.EnemyLayer;
    }

    protected virtual void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        if (agente == null)
        {
            Debug.LogError($"{name} requiere NavMeshAgent.");
            enabled = false;
            return;
        }

        agente.stoppingDistance = distanciaAlJugador;
        agente.autoBraking = true;

        if (petManager != null)
        {
            agente.speed = petManager.Velocidad;
            agente.acceleration = petManager.Aceleracion;
            agente.angularSpeed = petManager.Giro;
        }

        agente.avoidancePriority = UnityEngine.Random.Range(0, 100);
    }

    protected virtual void Update()
    {
        if (jugador == null || agente == null || !agente.isOnNavMesh)
            return;

        DetectarEnemigoCercano();
        Vector3 destino = CalcularDestino();

        if (Vector3.Distance(agente.destination, destino) > 0.5f)
            agente.SetDestination(destino);

        ComportamientoPersonalizado();
    }

    protected virtual void DetectarEnemigoCercano()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, rangoDeteccion, EnemyLayer);
        float distanciaMin = Mathf.Infinity;
        enemigoActual = null;

        foreach (var col in cols)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < distanciaMin)
            {
                distanciaMin = dist;
                enemigoActual = col.transform;
            }
        }
    }

    protected virtual Vector3 CalcularDestino()
    {
        if (enemigoActual != null)
            return enemigoActual.position;

        if (jugador != null)
            return jugador.position;

        return transform.position;
    }

    protected abstract void ComportamientoPersonalizado();
}
