using UnityEngine;

/// <summary>
/// Script del proyectil que maneja movimiento, colisión, daño, sonido y efecto visual.
/// Al impactar, desactiva render y collider pero espera a destruirse tras el sonido.
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(AudioSource))]
public class bulletB : MonoBehaviour
{
    [Header("Parámetros")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private ParticleSystem despawnEffect;
    [SerializeField] private AudioClip impactSound;

    private AudioSource audioSource;
    private bool isDestroyed = false;

    /// <summary>
    /// Inicializa el AudioSource y autodestrucción por tiempo.
    /// </summary>
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, lifetime);
    }

    /// <summary>
    /// Mueve la bala hacia adelante cada frame.
    /// </summary>
    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    /// <summary>
    /// Detecta colisión con enemigo, aplica daño, ejecuta efectos y oculta visualmente.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (isDestroyed) return;
        if (((1 << other.gameObject.layer) & enemyLayer.value) == 0) return;

        var stats = other.GetComponent<EnemiesController>();
        if (stats != null)
            stats.TakeDamage(damage);

        isDestroyed = true;

        // 🔥 Oculta renderers y collider
        GetComponent<Collider>().enabled = false;
        foreach (var renderer in GetComponentsInChildren<Renderer>())
            renderer.enabled = false;

        // 🔥 Lanza efecto de partícula
        if (despawnEffect != null)
            Instantiate(despawnEffect, transform.position, Quaternion.identity);

        // 🔥 Reproduce sonido y destruye al terminar
        if (impactSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(impactSound);
            Destroy(gameObject, impactSound.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
