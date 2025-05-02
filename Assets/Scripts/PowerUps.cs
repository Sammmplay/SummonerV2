using UnityEngine;
using System.Reflection;

/// <summary>
/// Administra powerups: mejora stats del jugador e invoca mascotas a través de PetManager.
/// </summary>
public class PowerUpManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject jugador;
    public PetManager petManager;

    [Header("Prefabs de mascotas")]
    public GameObject prefabAssassin;
    public GameObject prefabAttacker;
    public GameObject prefabDefender;

    [Header("Incrementos de stats")]
    public float saludExtra = 50f;
    public float healingAmount = 30f;
    public float velocidadExtra = 2f;
    public float areaExtra = 1f;

    // ----------- MÉTODOS PARA MEJORAR STATS DEL JUGADOR -----------

    public void ImproveHealth() => ModificarVariable("salud", saludExtra);
    public void Healing() => ModificarVariable("salud", healingAmount);
    public void ImproveVelocity() => ModificarVariable("velocidad", velocidadExtra);
    public void ImproveArea() => ModificarVariable("alcance", areaExtra);

    /// <summary>
    /// Busca y modifica dinámicamente una variable del jugador.
    /// </summary>
    private void ModificarVariable(string nombreCampo, float cantidad)
    {
        foreach (var comp in jugador.GetComponents<MonoBehaviour>())
        {
            var campo = comp.GetType().GetField(nombreCampo, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (campo != null)
            {
                if (campo.FieldType == typeof(int))
                    campo.SetValue(comp, (int)campo.GetValue(comp) + (int)cantidad);
                else if (campo.FieldType == typeof(float))
                    campo.SetValue(comp, (float)campo.GetValue(comp) + cantidad);

                Debug.Log($"Modificado {nombreCampo} en +{cantidad}");
                return;
            }
        }
        Debug.LogWarning($"No se encontró el campo '{nombreCampo}' en el jugador.");
    }

    // ----------- MÉTODOS PARA AÑADIR MASCOTAS -----------

    public void SummonAssassin() => AgregarMascota(prefabAssassin, "Assassin");
    public void SummonAttacker() => AgregarMascota(prefabAttacker, "Attacker");
    public void SummonDefender() => AgregarMascota(prefabDefender, "Defender");

    private void AgregarMascota(GameObject prefab, string nombre)
    {
        if (petManager != null && prefab != null)
        {
            petManager.AgregarMascotaRuntime(prefab, nombre);
        }
        else
        {
            Debug.LogWarning($"No se pudo invocar la mascota '{nombre}'. Verifica prefab y PetManager.");
        }
    }
}
