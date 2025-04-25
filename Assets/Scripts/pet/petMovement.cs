using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

/// <summary>
/// Mascotas que orbitan al jugador e invocan proyectiles (spawns) si detectan enemigos cerca.
/// No disparan realmente, solo instancian el prefab del proyectil frente a ellas.
/// Código modular y simple siguiendo la metodología KISSGPT.
/// </summary>
public class PetManager : MonoBehaviour
{
    // --- Referencias esenciales ---
    public Transform hero;                 // A quién siguen las mascotas
    public GameObject projPrefab;          // Proyectil que se instancia (no se dispara)
    public Transform evilNest;             // Padre que contiene todos los enemigos

    // --- Configuración de movimiento ---
    public float orbitMin = 2f;            // Distancia mínima al jugador
    public float orbitMax = 3.5f;          // Distancia máxima al jugador
    public float zoomSpd = 3f;             // Velocidad de movimiento

    // --- Detección y spawn ---
    public float evilRange = 8f;           // Distancia para detectar enemigos
    public float spawnLuck = 1f;           // Probabilidad de instanciar un proyectil por segundo

    void Start()
    {
        SetRandomPetSizes();
    }

    void Update()
    {
        UpdateAllPets();
    }

    /// <summary>
    /// Asigna un tamaño aleatorio a cada mascota para dar variedad visual.
    /// </summary>
    void SetRandomPetSizes()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform pet = transform.GetChild(i);
            float size = Random.Range(0.85f, 1.3f);
            pet.localScale = Vector3.one * size;
            Debug.Log("Tamaño aleatorio asignado a: " + pet.name);
        }
    }

    /// <summary>
    /// Actualiza el comportamiento de todas las mascotas en cada frame.
    /// </summary>
    void UpdateAllPets()
    {
        bool evilNear = IsEvilNearby();

        int total = transform.childCount;
        for (int i = 0; i < total; i++)
        {
            Transform pet = transform.GetChild(i);
            Vector3 orbitSpot = GetOrbitPosition(i, total);

            MovePet(pet, orbitSpot);
            TurnPet(pet, evilNear);
            TrySpawnProjectile(pet, evilNear);
        }
    }

    /// <summary>
    /// Verifica si algún enemigo está cerca del héroe.
    /// </summary>
    bool IsEvilNearby()
    {
        foreach (Transform evil in evilNest)
        {
            float dist = Vector3.Distance(hero.position, evil.position);
            if (dist <= evilRange) return true;
        }
        return false;
    }

    /// <summary>
    /// Calcula la posición orbital para cada mascota.
    /// </summary>
    Vector3 GetOrbitPosition(int index, int total)
    {
        float angle = (360f / total) * index;
        Vector3 direction = Quaternion.Euler(0, angle, 0) * hero.forward;
        float radius = Random.Range(orbitMin, orbitMax);
        return hero.position + direction * radius;
    }

    /// <summary>
    /// Mueve la mascota suavemente hacia su posición orbital.
    /// </summary>
    void MovePet(Transform pet, Vector3 destination)
    {
        pet.position = Vector3.MoveTowards(pet.position, destination, zoomSpd * Time.deltaTime);
    }

    /// <summary>
    /// Hace que la mascota mire al jugador o al centro de los enemigos.
    /// </summary>
    void TurnPet(Transform pet, bool evilNear)
    {
        Vector3 target = evilNear ? GetEvilCenter() : hero.position;
        Vector3 look = target - pet.position;
        if (look != Vector3.zero)
            pet.rotation = Quaternion.Slerp(pet.rotation, Quaternion.LookRotation(look), 5f * Time.deltaTime);
    }

    /// <summary>
    /// Si hay enemigos cerca, intenta invocar (instanciar) proyectiles desde la mascota.
    /// </summary>
    void TrySpawnProjectile(Transform pet, bool evilNear)
    {
        if (evilNear && Random.value < spawnLuck * Time.deltaTime)
        {
            StartCoroutine(SpawnProjectileBurst(pet));
            Debug.Log("Instanciando proyectil desde: " + pet.name);
        }
    }

    /// <summary>
    /// Calcula el punto medio entre los enemigos dentro del rango.
    /// </summary>
    Vector3 GetEvilCenter()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (Transform evil in evilNest)
        {
            if (Vector3.Distance(hero.position, evil.position) <= evilRange)
            {
                sum += evil.position;
                count++;
            }
        }

        if (count == 0) return hero.position;
        return sum / count;
    }

    /// <summary>
    /// Instancia entre 1 y 3 proyectiles frente a la mascota con una breve pausa entre cada uno.
    /// </summary>
    IEnumerator SpawnProjectileBurst(Transform pet)
    {
        int times = Random.Range(1, 4);
        for (int i = 0; i < times; i++)
        {
            Instantiate(projPrefab, pet.position, pet.rotation);
            Debug.Log("Proyectil #" + (i + 1) + " instanciado por " + pet.name);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Dibuja en el editor un círculo que representa el área de detección de enemigos.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        DrawVisionRadius();
    }

    /// <summary>
    /// Muestra la zona de detección como una esfera roja semitransparente en la escena.
    /// </summary>
    void DrawVisionRadius()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(hero != null ? hero.position : transform.position, evilRange);
    }
}