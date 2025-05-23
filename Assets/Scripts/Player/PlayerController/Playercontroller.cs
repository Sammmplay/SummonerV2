using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour
{
    public static Playercontroller Instance;
    //public GameObject character;
    Transform targetCamera;
    public Vector2 _input;
    public float _velocity = 2.6f;
    public float smoothRotation = 10f;
    Rigidbody _rb;
    public float characterHP = 10f;
    public float currentCharacterHP;


    public bool invulnerable = false;
    public float invulnerableCooldown = 2f;

    private WavesUI wavesUI;
    [Header("Animaciones")]
    Animator _anim;

    UI_Manager manager;

    //Bloqueo de movimiento
    bool bloqueado = false;
    private void Start()
    {
        manager = FindFirstObjectByType<UI_Manager>();

        wavesUI = FindFirstObjectByType<WavesUI>();

        _rb = GetComponent<Rigidbody>();
        targetCamera = Camera.main.transform;
        currentCharacterHP = characterHP;
    }
    private void Awake() {
        _anim = GetComponent<Animator>();
    }
    private void Update() {
        if (!bloqueado) {
            Move();
        }

        if (invulnerable)
        {
            invulnerableCooldown -= Time.deltaTime;
            if (invulnerableCooldown < 0)
            {
                invulnerable = false;
            }
        }
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
        //aplicamos la velocidad a nuestro parametro en el animator
        float vel = _rb.linearVelocity.magnitude;
        _anim.SetFloat("Velocity", vel);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy") && invulnerable == false)
        {
            currentCharacterHP -= collision.GetComponent<EnemiesController>().buffedDamage;
            wavesUI.TextUpdate();
            if (currentCharacterHP <= 0)
            {
                //character.SetActive(false);
                Dead();
                Debug.Log("Muerte");

                //Queda Poner un panel de UI para volver a la aldea o pelear de nuevo
            }
            invulnerableCooldown = 2f;
            invulnerable = true;
        }
    }
    void Dead() {
        this.enabled = false;
        //�Que mierda es esto que hacer que el PJ se caiga al infinito cuando la endi�e?
        Collider col = this.GetComponent<Collider>();
        col.enabled = false;
        _rb.isKinematic = true;
        _anim.SetTrigger("Dead");
        //Aqui se llama al men� de ENDpantalla, la funci�n "menuLose"
        manager.menuLose();
    }
    public void RespawnPlayer() {
        
        Collider col = this.GetComponent<Collider>();
        col.enabled = true;
       
    }
    public void AnimPickUpItem() {
        bloqueado = true; //bloquea el movimiento
        _rb.linearVelocity *= 0.2f;
        _anim.SetTrigger("PickUp");
    }
    public void RotacionPlayerAObjetoColeccionable(Transform target) {
        //direccion desde el jugador hacia el objeto 
        Vector3 dirTarget = (target.position - transform.position).normalized;
        //nos aseguramos de que no tenga componente vertical
        dirTarget.y = 0;
        //si la direccion es valida (no es cero)
        if (dirTarget.sqrMagnitude > 0.01f) {
            Quaternion rotTarget = Quaternion.LookRotation(dirTarget);
            //asigna directamente la rotacion(instantanea)
            //transform.rotation = rotTarget;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, Time.deltaTime * 90.0f);
        }
    }
    //evento que se llamara en el final de la animacion
    public void DesblkoquearMovimiento() {
        bloqueado = false;
    }

    public void BloquearMovimiento()
    {
        bloqueado = true;
    }
}
