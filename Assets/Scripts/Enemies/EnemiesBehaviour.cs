using UnityEngine;

public class EnemiesBehaviour : MonoBehaviour
{
    public GameObject enemyObjetive;
    private Rigidbody objetivePosition;
    public float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objetivePosition = enemyObjetive.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 direction = (objetivePosition.position - rb.position).normalized;
        Debug.Log(objetivePosition.position - rb.position);
        Vector3 targetVelocity = direction * speed;

        // Opci�n 1: Usar fuerza
        // rb.AddForce(targetVelocity, ForceMode.VelocityChange);

        // Opci�n 2: Cambiar velocidad
        rb.linearVelocity = targetVelocity;
    }
}