using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// ShieldB controla el escudo invocado por PetGuardian.
/// Escala el área visual (daño divino) y aplica daño + empuje a enemigos.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ShieldB : MonoBehaviour
{
    [Header("Parámetros de daño y empuje")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float pushForce = 2f;
    [SerializeField] private float growDuration = 0.3f;
    [SerializeField] private float stayDuration = 0.5f;
    [SerializeField] private float shrinkDuration = 0.3f;
    [SerializeField] private AudioClip impactSound;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Referencia visual (daño divino)")]
    [SerializeField] private Transform areaObject;  // Asigna el GameObject de partículas
    [SerializeField] private float finalScale = 2.5f;  // Escala máxima visible

    public Action OnShieldDestroyed;

    private AudioSource audioSource;
    private bool hasHit = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (areaObject == null)
        {
            Debug.LogError("ShieldB → Asigna el áreaObject (daño divino) en el inspector.");
            return;
        }

        areaObject.localScale = Vector3.zero;

        // Crece hasta el tamaño final
        LeanTween.scale(areaObject.gameObject, Vector3.one * finalScale, growDuration)
                 .setOnComplete(() => StartCoroutine(StayAndShrink()));
    }

    private IEnumerator StayAndShrink()
    {
        yield return new WaitForSeconds(stayDuration);

        // Encoge hasta desaparecer
        LeanTween.scale(areaObject.gameObject, Vector3.zero, shrinkDuration)
                 .setOnComplete(() =>
                 {
                     OnShieldDestroyed?.Invoke();
                     Destroy(gameObject);
                 });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        if (((1 << other.gameObject.layer) & enemyLayer.value) == 0) return;

        var stats = other.GetComponent<EnemyStats>();
        if (stats != null)
        {
            stats.TakeDamage(damage);

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 pushDir = (other.transform.position - transform.position).normalized;
                rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
            }

            if (impactSound != null && audioSource != null)
                audioSource.PlayOneShot(impactSound);
        }

        hasHit = true;
    }
}
