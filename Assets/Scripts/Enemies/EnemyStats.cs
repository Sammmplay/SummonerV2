using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Estadísticas del Enemigo")]
    public float Speed = 5f;
    public float MaxHealth = 10f;
    public float CurrentHealth = 10f;
    public float Defense = 0f;

    [Header("Animación")]
    public float hitReactionDistance = 0.2f;
    public float deathShrinkTime = 1f;

    private bool isDead = false;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        float damageTaken = Mathf.Max(amount - Defense, 0f);
        CurrentHealth -= damageTaken;

        // Reacción al ser golpeado
        HitReaction();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void HitReaction()
    {
        Vector3 recoilDirection = (transform.position - Camera.main.transform.position).normalized;
        Vector3 recoilTarget = transform.position + recoilDirection * hitReactionDistance;

        LeanTween.move(transform.gameObject, recoilTarget, 0.1f)
                 .setEase(LeanTweenType.easeOutQuad)
                 .setOnComplete(() =>
                 {
                     LeanTween.move(transform.gameObject, originalPosition, 0.1f)
                              .setEase(LeanTweenType.easeInQuad);
                 });
    }

    private void Die()
    {
        isDead = true;

        // Girar y encoger
        LeanTween.rotateAround(transform.gameObject, Vector3.up, 360f, deathShrinkTime);
        LeanTween.scale(gameObject, Vector3.zero, deathShrinkTime)
                 .setEase(LeanTweenType.easeInBack)
                 .setOnComplete(() => Destroy(gameObject));
    }
}

