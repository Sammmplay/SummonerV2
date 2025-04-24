using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PetManager : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public Transform enemiesParent;

    [Header("Colliders de Rango (Is Trigger)")]
    public Collider rangeMinCollider; // cualquier collider en modo Is Trigger
    public Collider rangeMaxCollider; // cualquier collider en modo Is Trigger

    [Header("Distancias")]
    public float minDistance = 1.5f;  // distancia interior m�nima
    public float maxDistance = 4f;    // distancia exterior m�xima

    [Header("Velocidades")]
    public Vector2 speedRange = new Vector2(1f, 5f);

    [Header("Separaci�n y Rotaci�n")]
    public float separationForce = 1f;
    public float rotationSpeed = 5f;

    [Header("Flotaci�n (LeanTween)")]
    public float floatAmplitude = 0.3f;
    public float floatDuration = 1.5f;

    // ----------------------------------------------
    // Estructura interna para cada mascota
    struct PetData
    {
        public Transform trans;
        public float speed;
    }
    List<PetData> pets = new List<PetData>();
    List<Transform> enemiesInRange = new List<Transform>();

    Vector3 lastPlayerPos;
    bool playerIsMoving;

    void Awake()
    {
        // A�adimos el script RangeTrigger a los dos colliders de rango
        rangeMinCollider.gameObject
            .AddComponent<RangeTrigger>()
            .Init(this, RangeTrigger.Type.RangoMin);
        rangeMaxCollider.gameObject
            .AddComponent<RangeTrigger>()
            .Init(this, RangeTrigger.Type.RangoMax);
    }

    void Start()
    {
        lastPlayerPos = player.position;
        InitializePets();
    }

    void Update()
    {
        // 1) Detectar movimiento del jugador
        playerIsMoving = (player.position - lastPlayerPos).sqrMagnitude > 0.0001f;
        lastPlayerPos = player.position;

        // 2) Para cada mascota: Mover, Rotar, Separar
        foreach (var pet in pets)
        {
            // Buscar el enemigo m�s cercano (solo en enemiesInRange)
            Transform nearest = FindNearestEnemy(pet.trans);

            // Definir target: mitad entre player y enemigo, o posici�n aleatoria v�lida
            Vector3 target = nearest != null
                ? Vector3.Lerp(player.position, nearest.position, 0.5f)
                : GetRandomPositionAround(player.position, pet.trans.position);

            MovePet(pet, target);
            RotatePet(pet, nearest);
            ApplySeparation(pet);
        }
    }

    // Inicializa lista de mascotas y a�ade flotaci�n
    void InitializePets()
    {
        pets.Clear();
        foreach (Transform child in transform)
        {
            // Ignorar los colliders de rango (pueden ser hijos)
            if (child == rangeMinCollider.transform || child == rangeMaxCollider.transform)
                continue;

            var pd = new PetData
            {
                trans = child,
                speed = Random.Range(speedRange.x, speedRange.y)
            };
            pets.Add(pd);

            // Efecto de flotaci�n con LeanTween (vaiv�n en Y)
            LeanTween.moveY(child.gameObject,
                child.position.y + floatAmplitude,
                floatDuration)
                .setLoopPingPong()
                .setEase(LeanTweenType.easeInOutSine);
        }
    }

    // � Callbacks desde RangeTrigger � 

    public void OnRangeTriggerEnter(RangeTrigger.Type type, Transform other)
    {
        bool isPet = other.parent == transform;
        bool isEnemy = other.parent == enemiesParent;

        if (type == RangeTrigger.Type.RangoMax && isEnemy)
        {
            // Un enemigo entra al rango m�ximo: lo guardamos
            if (!enemiesInRange.Contains(other))
                enemiesInRange.Add(other);
        }
        else if (type == RangeTrigger.Type.RangoMin && isPet)
        {
            // Una pet penetra demasiado: la empujamos hacia fuera al minDistance
            Vector3 dir = (other.position - player.position).normalized;
            other.position = player.position + dir * minDistance;
        }
    }

    public void OnRangeTriggerExit(RangeTrigger.Type type, Transform other)
    {
        bool isPet = other.parent == transform;
        bool isEnemy = other.parent == enemiesParent;

        if (type == RangeTrigger.Type.RangoMax)
        {
            if (isEnemy)
                enemiesInRange.Remove(other);
            else if (isPet)
            {
                // Una pet se sale de la jaula exterior: la traemos al maxDistance
                Vector3 dir = (other.position - player.position).normalized;
                other.position = player.position + dir * maxDistance;
            }
        }
    }

    // � L�gica de movimiento, rotaci�n y separaci�n � 

    Transform FindNearestEnemy(Transform pet)
    {
        Transform closest = null;
        float bestSqr = maxDistance * maxDistance;
        foreach (var e in enemiesInRange)
        {
            float sqr = (e.position - pet.position).sqrMagnitude;
            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                closest = e;
            }
        }
        return closest;
    }

    Vector3 GetRandomPositionAround(Vector3 center, Vector3 petPos)
    {
        Vector3 dir = (petPos - center).normalized;
        if (dir == Vector3.zero) dir = Random.insideUnitSphere.normalized;
        float dist = Mathf.Clamp((petPos - center).magnitude, minDistance, maxDistance);
        return center + dir * dist;
    }

    void MovePet(PetData pet, Vector3 target)
    {
        float distToPlayer = Vector3.Distance(pet.trans.position, player.position);
        if (distToPlayer > minDistance || playerIsMoving)
        {
            pet.trans.position = Vector3.MoveTowards(
                pet.trans.position, target, pet.speed * Time.deltaTime);
        }
    }

    void RotatePet(PetData pet, Transform enemy)
    {
        Vector3 lookDir = ((enemy != null ? enemy.position : player.position)
                            - pet.trans.position);
        lookDir.y = 0;
        if (lookDir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(lookDir.normalized);
        pet.trans.rotation = Quaternion.Slerp(
            pet.trans.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    void ApplySeparation(PetData pet)
    {
        foreach (var other in pets)
        {
            if (other.trans == pet.trans) continue;
            float sq = (other.trans.position - pet.trans.position).sqrMagnitude;
            if (sq < 1f)
            {
                Vector3 push = (pet.trans.position - other.trans.position).normalized;
                pet.trans.position += push * separationForce * Time.deltaTime;
            }
        }
    }

    // � Clase interna para disparar los triggers de rango � 

    public class RangeTrigger : MonoBehaviour
    {
        public enum Type { RangoMin, RangoMax }
        public Type type;
        PetManager manager;

        public RangeTrigger Init(PetManager mgr, Type t)
        {
            manager = mgr;
            type = t;
            return this;
        }

        void OnTriggerEnter(Collider other)
            => manager.OnRangeTriggerEnter(type, other.transform);

        void OnTriggerExit(Collider other)
            => manager.OnRangeTriggerExit(type, other.transform);
    }
}
