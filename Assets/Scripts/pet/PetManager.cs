using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

/// <summary>
/// Administra el spawn, configuración y gestión de mascotas activas.
/// </summary>
public class PetManager : MonoBehaviour
{
    [Header("Referencias globales")]
    [SerializeField] private Transform jugador;
    [SerializeField] private GameObject proyectilBase;

    [Header("Prefabs iniciales")]
    [SerializeField] private List<GameObject> mascotasIniciales;

    [Header("Capas y área")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask petsLayer;
    [SerializeField] private float areaRadius = 5f;

    [Header("NavMesh config global")]
    [SerializeField] private float velocidad = 3.5f;
    [SerializeField] private float aceleracion = 8f;
    [SerializeField] private float giro = 120f;

    [Header("Escala de spawn")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;

    [System.NonSerialized] public List<GameObject> mascotasActivas = new List<GameObject>();

    public float AreaRadius => areaRadius;
    public LayerMask EnemyLayer => enemyLayer;
    public LayerMask PetsLayer => petsLayer;
    public float Velocidad => velocidad;
    public float Aceleracion => aceleracion;
    public float Giro => giro;

    private void Start()
    {
        SpawnearMascotasIniciales();
    }

    private void SpawnearMascotasIniciales()
    {
        foreach (var prefab in mascotasIniciales)
        {
            if (prefab != null)
                AgregarMascotaRuntime(prefab, prefab.name);
        }
    }

    public GameObject AgregarMascotaRuntime(GameObject prefab, string nombre)
    {
        if (NavMesh.SamplePosition(jugador.position + UnityEngine.Random.insideUnitSphere * areaRadius, out NavMeshHit hit, areaRadius, NavMesh.AllAreas))
        {
            GameObject instancia = Instantiate(prefab, hit.position, Quaternion.identity);
            instancia.name = nombre;

            float targetScale = UnityEngine.Random.Range(minScale, maxScale);
            instancia.transform.localScale = Vector3.one * targetScale;

            var pet = instancia.GetComponent<PetBase>();
            if (pet != null)
                pet.Configurar(jugador, proyectilBase, this);

            mascotasActivas.Add(instancia);
            Debug.Log($"Mascota añadida: {nombre}");
            return instancia;
        }

        Debug.LogWarning($"No se pudo spawnear la mascota '{nombre}': fuera del NavMesh.");
        return null;
    }

    public void RemoverMascotaPorNombre(string nombre)
    {
        LimpiarListaMascotas();

        GameObject mascotaARemover = mascotasActivas.Find(m => m != null && m.name.Contains(nombre));
        if (mascotaARemover != null)
        {
            mascotasActivas.Remove(mascotaARemover);
            Destroy(mascotaARemover);
            Debug.Log($"Mascota eliminada: {nombre}");
        }
        else
        {
            Debug.LogWarning($"No se encontró mascota activa con el nombre: {nombre}");
        }
    }

    public void RemoverTodasLasMascotas()
    {
        foreach (var mascota in mascotasActivas)
        {
            if (mascota != null)
                Destroy(mascota);
        }
        mascotasActivas.Clear();
        Debug.Log("Todas las mascotas fueron eliminadas.");
    }

    private void LimpiarListaMascotas()
    {
        mascotasActivas.RemoveAll(m => m == null);
    }

    public void AjustarAreaRadius(float nuevoRadio)
    {
        areaRadius = nuevoRadio;
    }

    private void OnDrawGizmosSelected()
    {
        if (jugador == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(jugador.position + Vector3.up, areaRadius);
    }
}
