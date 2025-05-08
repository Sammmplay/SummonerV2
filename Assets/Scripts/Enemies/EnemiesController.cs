using UnityEngine;
using UnityEngine.AI;

public class EnemiesController : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;

    public float baseDamage = 1;
    public float buffedDamage;
    public float baseHP = 1;
    public float buffedHP;
    public float currentHP;

    public float petDamage;

    [Header("Animaci�n")]
    public float hitReactionDistance = 0.2f;
    public float deathShrinkTime = 1f;

    private Vector3 originalPosition;

    private bool isDead = false;

    public WaveController waveController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject player = GameObject.FindWithTag("Player");
        waveController = GetComponent<WaveController>();
        if (player != null)
        {
            target = player.transform;
        }

        buffedDamage = baseDamage;
        buffedHP = baseHP;
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHP -= petDamage;

        // Reacci�n al ser golpeado
        HitReaction();

        if (currentHP <= 0)
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
        waveController.defeatedEnemies += 1;
        isDead = true;

        // Girar y encoger
        LeanTween.rotateAround(transform.gameObject, Vector3.up, 360f, deathShrinkTime);
        LeanTween.scale(gameObject, Vector3.zero, deathShrinkTime)
                 .setEase(LeanTweenType.easeInBack)
                 .setOnComplete(() => Destroy(gameObject));
    }
}