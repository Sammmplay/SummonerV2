using UnityEngine;

/// <summary>
/// Componente b�sico de proyectil.
/// No se mueve solo ni aplica da�o.
/// El movimiento, da�o y l�gica son controlados por la pet.
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

