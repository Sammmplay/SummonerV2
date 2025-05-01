using UnityEngine;

/// <summary>
/// Central system for PowerUps:
/// - Modifies player stats (health, speed)
/// - Upgrades pet stats (Attacker, Assassin, Guardian)
/// - Adds or removes pets at runtime
/// Compatible with shop buttons and UI.
/// </summary>
public class PowerUps : MonoBehaviour
{
    [Header("Main References")]
    [Tooltip("Reference to the Playercontroller script.")]
    [SerializeField] private Playercontroller player;

    [Tooltip("Reference to the PetManager in the scene.")]
    [SerializeField] private PetManager petManager;

    [Header("Pet Prefabs")]
    [Tooltip("Attacker prefab to spawn dynamically.")]
    [SerializeField] private GameObject prefabAttacker;

    [Tooltip("Assassin prefab to spawn dynamically.")]
    [SerializeField] private GameObject prefabAssassin;

    [Tooltip("Defender prefab to spawn dynamically.")]
    [SerializeField] private GameObject prefabDefender;

    // -------------------- PLAYER --------------------

    /// <summary>
    /// Increase player health.
    /// </summary>
    public void IncreasePlayerHealth(float amount)
    {
        player.characterHP += amount;
    }

    /// <summary>
    /// Increase player movement speed.
    /// </summary>
    public void IncreasePlayerSpeed(float amount)
    {
        player._velocity += amount;
    }

    /// <summary>
    /// Increase spawn and detection radius in PetManager.
    /// </summary>
    public void IncreaseArea(float amount)
    {
        petManager.AjustarSpawnAndDetectRadius(petManager.SpawnAndDetectRadius + amount);
    }

    // -------------------- UPGRADE PETS (ADVANCED WITH PARAMETERS) --------------------

    public void UpgradeAttacker(float extraDamage, float extraFireRate, float extraProjectileSpeed)
    {
        foreach (PetAttacker pet in Object.FindObjectsByType<PetAttacker>(FindObjectsSortMode.None))
        {
            pet.damage += extraDamage;
            pet.attackInterval = Mathf.Max(0.1f, pet.attackInterval - extraFireRate);
            pet.projectileSpeed += extraProjectileSpeed;
        }
    }

    public void UpgradeAssassin(float extraDamage, float extraDashSpeed, float dashDelayReduction)
    {
        foreach (PetAssassin pet in Object.FindObjectsByType<PetAssassin>(FindObjectsSortMode.None))
        {
            pet.damage += extraDamage;
            pet.dashSpeed += extraDashSpeed;
            pet.reboteDelay = Mathf.Max(0.05f, pet.reboteDelay - dashDelayReduction);
        }
    }

    public void UpgradeDefender(float extraPushDamage)
    {
        foreach (PetGuardian pet in Object.FindObjectsByType<PetGuardian>(FindObjectsSortMode.None))
        {
            pet.damage += extraPushDamage;
        }
    }

    // -------------------- UPGRADE PETS (NO PARAMETERS, FOR BUTTONS) --------------------

    /// <summary>
    /// Upgrade Attackers with default values.
    /// </summary>
    public void UpgradeAttacker()
    {
        UpgradeAttacker(1f, 0.1f, 2f);
    }

    /// <summary>
    /// Upgrade Assassins with default values.
    /// </summary>
    public void UpgradeAssassin()
    {
        UpgradeAssassin(1f, 5f, 0.1f);
    }

    /// <summary>
    /// Upgrade Defenders with default values.
    /// </summary>
    public void UpgradeDefender()
    {
        UpgradeDefender(1f);
    }

    // -------------------- ADD PETS --------------------

    public void AddAttacker() => petManager.SpawnearPrefab(prefabAttacker);
    public void AddAssassin() => petManager.SpawnearPrefab(prefabAssassin);
    public void AddDefender() => petManager.SpawnearPrefab(prefabDefender);

    // -------------------- REMOVE PETS --------------------

    public void RemoveAttacker() => petManager.RemoverMascota<PetAttacker>();
    public void RemoveAssassin() => petManager.RemoverMascota<PetAssassin>();
    public void RemoveDefender() => petManager.RemoverMascota<PetGuardian>();
}
