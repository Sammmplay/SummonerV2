using UnityEngine;
using System.Collections.Generic;

public class PetMovement : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform enemiesParent;

    [Header("Movement Settings")]
    public float minDistance = 1.5f;
    public float maxDistance = 4f;
    public Vector2 speedRange = new Vector2(1f, 5f);
    public float checkInterval = 1.5f;

    [Header("Enemy Detection")]
    public float enemyDetectionRadius = 6f;

    [Header("Behavior")]
    public float separationForce = 1f;
    public float rotationSmoothness = 5f;

    private List<Transform> pets = new List<Transform>();
    private Dictionary<Transform, float> moveSpeeds = new Dictionary<Transform, float>();
    private Vector3 lastPlayerPos;
    private bool playerIsMoving;
    private float timer;

    void Start()
    {
        UpdatePetList();
        lastPlayerPos = player.position;
    }

    void Update()
    {
        // Detectar si el jugador se ha movido
        playerIsMoving = Vector3.Distance(player.position, lastPlayerPos) > 0.01f;
        lastPlayerPos = player.position;

        // Verificar si se agregaron/eliminaron mascotas
        if (transform.childCount != pets.Count)
        {
            UpdatePetList();
        }

        // Actualizar comportamiento de mascotas
        UpdatePets();
    }

    void UpdatePetList()
    {
        pets.Clear();
        moveSpeeds.Clear();

        foreach (Transform pet in transform)
        {
            pets.Add(pet);
            moveSpeeds[pet] = Random.Range(speedRange.x, speedRange.y);
            AssignRandomSize(pet);
        }
    }

    void UpdatePets()
    {
        for (int i = 0; i < pets.Count; i++)
        {
            Transform pet = pets[i];
            Transform enemy = FindNearestEnemy(pet);
            float speed = moveSpeeds[pet];

            HandlePetBehavior(pet, enemy, speed);
            ApplySeparation(pet, i);
        }
    }

    /// <summary>
    /// Controla el comportamiento completo de la mascota.
    /// </summary>
    void HandlePetBehavior(Transform pet, Transform enemy, float speed)
    {
        Vector3 target;

        if (enemy != null)
        {
            // Posición entre el jugador y el enemigo
            target = Vector3.Lerp(player.position, enemy.position, 0.5f);
        }
        else
        {
            // Posición aleatoria dentro del rango
            target = GetPositionAroundPlayer(pet);
        }

        float distToPlayer = Vector3.Distance(pet.position, player.position);

        // Solo se mueve si está fuera del rango mínimo o si el jugador se está moviendo
        if (distToPlayer > minDistance || playerIsMoving)
        {
            pet.position = Vector3.MoveTowards(pet.position, target, speed * Time.deltaTime);
        }

        // Siempre mira al enemigo si está, si no, al jugador
        Vector3 lookTarget = (enemy != null ? enemy.position : player.position);
        Vector3 direction = lookTarget - pet.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction.normalized);
            pet.rotation = Quaternion.Slerp(pet.rotation, lookRot, Time.deltaTime * rotationSmoothness);
        }
    }

    /// <summary>
    /// Calcula una posición cercana al jugador con separación aleatoria.
    /// </summary>
    Vector3 GetPositionAroundPlayer(Transform pet)
    {
        Vector3 offset = (pet.position - player.position).normalized;
        if (offset == Vector3.zero) offset = Random.insideUnitSphere.normalized;

        float distance = Mathf.Clamp(Vector3.Distance(pet.position, player.position), minDistance, maxDistance);
        return player.position + offset * distance;
    }

    /// <summary>
    /// Encuentra al enemigo más cercano dentro del rango.
    /// </summary>
    Transform FindNearestEnemy(Transform pet)
    {
        Transform closest = null;
        float shortest = Mathf.Infinity;

        foreach (Transform enemy in enemiesParent)
        {
            float dist = Vector3.Distance(pet.position, enemy.position);
            if (dist < enemyDetectionRadius && dist < shortest)
            {
                closest = enemy;
                shortest = dist;
            }
        }

        return closest;
    }

    /// <summary>
    /// Separa mascotas muy cercanas entre sí.
    /// </summary>
    void ApplySeparation(Transform pet, int index)
    {
        for (int i = 0; i < pets.Count; i++)
        {
            if (i == index) continue;

            Transform other = pets[i];
            float distance = Vector3.Distance(pet.position, other.position);

            if (distance < 1f)
            {
                Vector3 push = (pet.position - other.position).normalized;
                pet.position += push * separationForce * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Asigna un tamaño aleatorio a la mascota.
    /// </summary>
    void AssignRandomSize(Transform pet)
    {
        float size = Random.Range(0.8f, 1.4f);
        pet.localScale = Vector3.one * size;
    }
}








