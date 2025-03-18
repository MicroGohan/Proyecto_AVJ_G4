using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [Header("Configuracion de Movimiento")]
    [Tooltip("Fuerza del movimiento del personaje.")]
    public int runSpeed = 1;

    [Header("Configuracion de Salto")]
    [Tooltip("Fuerza del salto del personaje.")]
    public int jumpStrength = 1;

    [Header("Límites de Movimiento")]
    [Tooltip("Límite inferior en el eje Y.")]
    public float minY = -3f;

    [Tooltip("Límite superior en el eje Y.")]
    public float maxY = -0.5f;

    //Movimiento
    float horizontal;
    float vertical;
    bool facingRight;

    //Animador
    Animator animator;

    //Salto
    Rigidbody2D cuerpoRigido;
    float axisY;
    bool isJump;

    // Layer para el suelo
    public Transform chequeoSuelo;
    public LayerMask capaSuelo;

    // Ataque
    bool isAttack;

    void Awake() // Metodo para comenzar las animaciones
    {
        animator = GetComponent<Animator>();
        cuerpoRigido = GetComponent<Rigidbody2D>();
        cuerpoRigido.Sleep();
    }

    void Update() // Acciones
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(horizontal != 0 ? horizontal : vertical));

        

    }

    void onLand() // Metodo para aterrizar
    {
        isJump = false;
        cuerpoRigido.gravityScale = 0f;
        cuerpoRigido.Sleep();
        axisY = transform.position.y;
        animator.SetBool("isJump", false);
    }

    void FixedUpdate() // Metodo para ejecutar acciones por frames
    {

        if(Input.GetButton("Fire1"))
        {
            isAttack = true;
            if (vertical != 0 || horizontal != 0)
            {
                vertical = 0;
                horizontal = 0;
                animator.SetFloat("Speed",0);
            }

            animator.SetTrigger("Attack1");
        }

        if(transform.position.y <= axisY && isJump)
        {
            onLand();
        }

        if(Input.GetButtonDown("Jump") && !isJump)
        {
            axisY = transform.position.y;
            isJump = true;
            cuerpoRigido.gravityScale = 1.5f;
            cuerpoRigido.WakeUp();
            cuerpoRigido.AddForce(new Vector2(0, jumpStrength));
            animator.SetBool("isJump", isJump);
        }

        if((vertical != 0 || horizontal != 0) && !isAttack)
        {
            Vector3 movement = new Vector3(horizontal * runSpeed, vertical * runSpeed, 0.0f);
            transform.position = transform.position + movement * Time.deltaTime;
        }

        // Calcula la nueva posición
        Vector3 newPosition = transform.position + new Vector3(horizontal * runSpeed, vertical * runSpeed, 0.0f) * Time.deltaTime;

        // Aplica los límites en el eje Y
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Asigna la nueva posición al personaje
        transform.position = newPosition;

        Flip(horizontal);

        
    }

    public void AlertObservers(string message)
    {
        if(message == "AttackEnd")
        {
            isAttack= false;
        }
    }

    private void Flip(float horizontal) // Metodo para hacer que el jugador vea a la izquierda
    {
        if(horizontal < 0 && !facingRight || horizontal > 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}