using System.Collections;
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
    Animator anim;
    public float hitReactionDistance = 0.2f;
    public float deathShrinkTime = 1f;

    private Vector3 originalPosition;

    private bool isDead = false;

    public WaveController waveController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        GameObject player = GameObject.FindWithTag("Player");
        waveController = FindFirstObjectByType<WaveController>();
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
            if (agent.velocity.magnitude > 0.1f) {
                anim.SetBool("Run", true);
            } else {
                anim.SetBool("Run", false);
            }
            if (!agent.enabled) return;
            agent.SetDestination(target.position);
            
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHP -= petDamage;
        
       

        if (currentHP <= 0)
        {
            Die();
        } else {
            // Reacci�n al ser golpeado
            HitReaction();
        }
    }

    private void HitReaction()
    {
        Debug.Log("Hit");
        anim.SetTrigger("Hit");
        agent.isStopped = true;
        agent.enabled = false;
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        Vector3 recoilDirection = (transform.position - Camera.main.transform.position).normalized;
        Vector3 recoilTarget = transform.position + recoilDirection * hitReactionDistance;

        LeanTween.move(transform.gameObject, recoilTarget, 0.1f)
                 .setEase(LeanTweenType.easeOutQuad)
                 .setOnComplete(() =>
                 {
                     LeanTween.move(transform.gameObject, originalPosition, 0.1f)
                              .setEase(LeanTweenType.easeInQuad);
                     Collider col = GetComponent<Collider>();
                     col.enabled = false;
                     agent.enabled = true;

                 });

    }

    public void Die()
    {
        Debug.Log("muerte");
        isDead = true;
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        agent.velocity = Vector3.zero;
        //agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.enabled = false;
        anim.SetTrigger("Dead");
        StartCoroutine(WaitingForDestroy());
        // Girar y encoger
        /*LeanTween.rotateAround(transform.gameObject, Vector3.up, 360f, deathShrinkTime);
        LeanTween.scale(gameObject, Vector3.zero, deathShrinkTime)
                 .setEase(LeanTweenType.easeInBack)
                 .setOnComplete(() => Destroy(gameObject));*/
    }
    IEnumerator WaitingForDestroy() {
        yield return new WaitForSeconds(3);
        waveController.defeatedEnemies ++;
        Destroy(gameObject);
    }
    public void ComenzarPersecucion() {
        agent.enabled = true;
        Collider col = GetComponent<Collider>();
        col.enabled = true;
    }
}