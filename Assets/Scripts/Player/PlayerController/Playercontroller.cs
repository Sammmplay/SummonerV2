using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour
{
    public static Playercontroller Instance;
     Transform targetCamera;
    public Vector2 _input;
    public float _velocity = 2.6f;
    public float smoothRotation = 10f;
    Rigidbody _rb;
    private void Start() {
        _rb = GetComponent<Rigidbody>();
        targetCamera = Camera.main.transform;
    }
    private void Update() {
        Move();
    }
    public void OnMove(InputValue value) {
        _input = value.Get<Vector2>();
    }
    
    public void Move() {
        Vector3 direction = new Vector3(_input.x,0, _input.y).normalized;
        if (direction.sqrMagnitude > 0.01f) {
            // calculamos la rotacion hacia la direccion 
            Quaternion targetRot = Quaternion.LookRotation(direction);
            //suavizamos la rotacion hacia el objetivo (giro fluido)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * smoothRotation);

        }
        //aplicamos movimiento al rigibody
        _rb.linearVelocity = direction * _velocity;
    }
}
