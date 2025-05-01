// PetManager.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

/// <summary>
/// Controlador central del sistema de mascotas.
/// - Usa spawnRadius como �rea de spawn y detecci�n.
/// - Genera tama�o aleatorio entre minScale y maxScale.
/// - Anima el spawn con LeanTween (escala + rotaci�n).
/// - Instancia y configura las mascotas, pas�ndoles referencias y lista de enemigos.
/// </summary>
public class PetManager : MonoBehaviour
{
    [Header("Referencias Globales")]
    [Tooltip("Transform del jugador que las mascotas seguir�n.")]
    [SerializeField] private Transform jugador;

    [Tooltip("Prefab de proyectil que usar�n Attacker/Assassin.")]
    [SerializeField] private GameObject proyectilBase;

    [Header("Prefabs de Mascotas")]
    [Tooltip("Prefabs de mascotas a instanciar.")]
    [SerializeField] private List<GameObject> mascotasIniciales;

    [Header("Capa y �rea")]
    [Tooltip("Capa asignada a los enemigos (OverlapSphere).")]
    [SerializeField] private LayerMask enemyLayer;

    [Tooltip("Capa asignada a las mascotas.")]
    [SerializeField] private LayerMask petsLayer;

    [Tooltip("Radio usado tanto para spawn como para detecci�n.")]
    [SerializeField] private float spawnRadius = 5f;

    [Header("Escala Aleatoria")]
    [Tooltip("Escala m�nima tras spawn.")]
    [SerializeField] private float minScale = 0.8f;

    [Tooltip("Escala m�xima tras spawn.")]
    [SerializeField] private float maxScale = 1.2f;

    private List<PetBase> mascotasActivas = new();
    private List<Transform> enemigosDetectados = new();

    private void Start()
    {
        SpawnearYConfigurarPets();
    }

    private void Update()
    {
        ActualizarEnemigosCercanos();
    }

    /// <summary>
    /// Instancia cada prefab en un punto aleatorio del NavMesh dentro de spawnRadius,
    /// aplica animaci�n de spawn y luego lo configura.
    /// </summary>
    private void SpawnearYConfigurarPets()
    {
        foreach (GameObject prefab in mascotasIniciales)
        {
            if (prefab == null) continue;

            // Determina posici�n v�lida en NavMesh
            Vector3 offset = Random.insideUnitSphere * spawnRadius;
            offset.y = 0;
            if (NavMesh.SamplePosition(jugador.position + offset, out NavMeshHit hit, spawnRadius, NavMesh.AllAreas))
            {
                GameObject instancia = Instantiate(prefab, hit.position, Quaternion.identity);

                // Tama�o aleatorio
                float targetScale = Random.Range(minScale, maxScale);
                instancia.transform.localScale = Vector3.zero;

                // Animaci�n de spawn con LeanTween
                float dur = 0.5f;
                LeanTween.scale(instancia, Vector3.one * targetScale, dur)
                         .setEaseOutBack();
                LeanTween.rotateY(instancia, 360f, dur)
                         .setEaseInOutQuad()
                         .setOnComplete(() => instancia.transform.rotation = Quaternion.identity);

                ConfigurarPet(instancia);
            }
        }
    }

    /// <summary>
    /// Inyecta referencias en la instancia de mascota.
    /// </summary>
    private void ConfigurarPet(GameObject instancia)
    {
        PetBase pet = instancia.GetComponent<PetBase>();
        if (pet == null) return;

        pet.Configurar(jugador, proyectilBase, enemigosDetectados, this);
        pet.EnemyLayer = enemyLayer;
        mascotasActivas.Add(pet);
    }

    /// <summary>
    /// Actualiza la lista de enemigos dentro de spawnRadius alrededor del jugador.
    /// </summary>
    private void ActualizarEnemigosCercanos()
    {
        enemigosDetectados.Clear();
        Vector3 centro = jugador.position + Vector3.up * 1f;
        Collider[] cols = Physics.OverlapSphere(centro, spawnRadius, enemyLayer);
        foreach (var c in cols)
            enemigosDetectados.Add(c.transform);
    }

    /// <summary>
    /// Dibuja en el Editor el �rea de spawn/detecci�n.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (jugador == null) return;
        Gizmos.color = Color.cyan;
        Vector3 centro = jugador.position + Vector3.up * 1f;
        Gizmos.DrawWireSphere(centro, spawnRadius);
    }

    /// <summary>
    /// Radio usado para spawn y detecci�n (read-only).
    /// </summary>
    public float SpawnAndDetectRadius => spawnRadius;

    /// <summary>
    /// Ajusta el radio en tiempo real (por ejemplo, power-up).
    /// </summary>
    public void AjustarSpawnAndDetectRadius(float nuevoRadio)
    {
        spawnRadius = nuevoRadio;
    }

    public LayerMask EnemyLayer => enemyLayer;
    public LayerMask PetsLayer => petsLayer;
    public void SpawnearPrefab(GameObject prefab)
    {
        if (NavMesh.SamplePosition(jugador.position + Random.insideUnitSphere * spawnRadius, out NavMeshHit hit, spawnRadius, NavMesh.AllAreas))
        {
            GameObject instancia = Instantiate(prefab, hit.position, Quaternion.identity);
            ConfigurarPet(instancia);
        }
    }

    public void RemoverMascota<T>() where T : PetBase
    {
        var pet = mascotasActivas.Find(p => p is T);
        if (pet != null)
        {
            mascotasActivas.Remove(pet);
            Destroy(pet.gameObject);
        }
    }

}
