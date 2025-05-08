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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject player = GameObject.FindWithTag("Player");
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
    public void Death() {

    }
}