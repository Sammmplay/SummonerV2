using UnityEngine;

/// <summary>
/// Componente básico de proyectil.
/// No se mueve solo ni aplica daño.
/// El movimiento, daño y lógica son controlados por la pet.
/// </summary>
public class ProjectileHandler : MonoBehaviour
{
    [Tooltip("Tiempo de vida en segundos antes de destruirse.")]
    public float Lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, Lifetime);
    }
}

