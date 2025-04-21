using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    [Header("Configuracion de Movimiento")]
    [Tooltip("Fuerza del movimiento del personaje.")]
    public int runSpeed = 1;

    [Header("Configuracion de Salto")]
    [Tooltip("Fuerza del salto del personaje.")]
    public int jumpStrength = 1;

    [Header("Limites de Movimiento")]
    [Tooltip("Limite inferior en el eje Y.")]
    public float minY = -3f;

    [Tooltip("Limite superior en el eje Y.")]
    public float maxY = -0.5f;

    [Header("Configuracion de Rebote")]
    [Tooltip("Fuerza que empuja al personaje cuando es dañado.")]
    public int bounceStrength = 1;

    [Header("Configuracion de Salud")]
    [Tooltip("Salud máxima del personaje.")]
    public int maxHealth = 3;

    [Tooltip("Referencia al texto TMP de las vidas.")]
    public TMP_Text Lives;

    [Tooltip("Tiempo de invencibilidad después de recibir daño.")]
    public float invincibilityTime = 1f;

    // Movimiento
    float horizontal;
    float vertical;
    bool facingRight;

    // Animador
    Animator animator;

    // Salto
    Rigidbody2D cuerpoRigido;
    float axisY;
    bool isJump;

    // Layer para el suelo
    public Transform chequeoSuelo;
    public LayerMask capaSuelo;

    // Ataque
    bool isAttack;

    // Daño
    public bool isAttacked;
    private int currentHealth; // Salud actual del personaje
    private bool isInvincible = false; // Indica si el personaje es invencible

    //public GameObject attackArea;
    private bool isAttacking = false;

    public PlayerAttackArea attackArea;

    private MusicManager musicManager;
    // Modifica el método TryAttack para activar/desactivar el área
    public void TryAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack1");
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.5f); // Duración del ataque
        isAttacking = false;
    }

    public void EnableAttackArea()
    {
        if (attackArea != null)
        {
            attackArea.EnableArea();
        }
        else
        {
            Debug.LogWarning("AttackArea no está asignada en " + gameObject.name);
        }
    }

    public void DisableAttackArea()
    {
        if (attackArea != null)
        {
            attackArea.DisableArea(); // Usa el método de PlayerAttackArea
        }
    }
    /*
    // Métodos para activar/desactivar el área (llamados desde Animation Events)
    public void EnableAttackArea()
    {
        attackArea.SetActive(true);
    }

    public void DisableAttackArea()
    {
        attackArea.SetActive(false);
    }*/
    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        if (musicManager == null)
        {
            Debug.LogWarning("No se encontró MusicManager en la escena.");
        }
    }
    void Awake() // Metodo para comenzar las animaciones
    {
        animator = GetComponent<Animator>();
        cuerpoRigido = GetComponent<Rigidbody2D>();
        cuerpoRigido.Sleep();
        currentHealth = maxHealth; // Inicializa la salud al máximo
        UpdateLivesText(); // Actualiza el texto de las vidas al inicio
                           // Busca el PlayerAttackArea en los hijos si no está asignado
        if (attackArea == null)
        {
            attackArea = GetComponentInChildren<PlayerAttackArea>();
            if (attackArea == null)
            {
                Debug.LogWarning("No se encontró PlayerAttackArea en los hijos del personaje");
            }
        }
    }

    void Update() // Acciones
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(horizontal != 0 ? horizontal : vertical));

        // Simula recibir daño al presionar la tecla "L" (solo para pruebas)
        if (Input.GetKeyDown(KeyCode.L))
        {
            beingAttacked(new Vector2(1, 0), 1); // Dirección: derecha, Daño: 1
        }
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
        if (Input.GetButton("Fire1") && !isJump && !isAttacked)
        {
            isAttack = true;
            if (vertical != 0 || horizontal != 0)
            {
                vertical = 0;
                horizontal = 0;
                animator.SetFloat("Speed", 0);
            }

            animator.SetTrigger("Attack1");
        }

        if (transform.position.y <= axisY && isJump)
        {
            onLand();
        }

        if (Input.GetButtonDown("Jump") && !isJump && !isAttacked)
        {
            axisY = transform.position.y;
            isJump = true;
            cuerpoRigido.gravityScale = 1.5f;
            cuerpoRigido.WakeUp();
            cuerpoRigido.AddForce(new Vector2(0, jumpStrength));
            animator.SetBool("isJump", isJump);
        }

        if ((vertical != 0 || horizontal != 0) && !isAttack && !isAttacked)
        {
            Vector3 movement = new Vector3(horizontal * runSpeed, vertical * runSpeed, 0.0f);
            transform.position = transform.position + movement * Time.deltaTime;
        }

        // Calcula la nueva posicion
        Vector3 newPosition = transform.position + new Vector3(horizontal * runSpeed, vertical * runSpeed, 0.0f) * Time.deltaTime;

        // Aplica los limites en el eje Y
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Asigna la nueva posicion al personaje
        transform.position = newPosition;

        Flip(horizontal);
    }

    public void AlertObservers(string message)
    {
        if (message == "AttackEnd")
        {
            isAttack = false;
        }
    }

    private void Flip(float horizontal) // Metodo para hacer que el jugador vea a la izquierda
    {
        if (horizontal < 0 && !facingRight || horizontal > 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    public void beingAttacked(Vector2 direccion, int DMG)
    {
        if (!isAttacked && !isInvincible)
        {
            // Reduce la salud
            currentHealth -= DMG;
            UpdateLivesText();

            // Verifica si el personaje murió
            if (currentHealth <= 0)
            {
                Die(); // Va directamente a la muerte sin activar la animación de daño
            }
            else
            {
                // Solo activa la animación de daño si no está muriendo
                isAttacked = true;
                animator.SetBool("isAttacked", isAttacked);

                // Empuja al personaje en la dirección del ataque
                Vector2 bounce = new Vector2(transform.position.x - direccion.x, 1).normalized;
                cuerpoRigido.AddForce(bounce * bounceStrength, ForceMode2D.Impulse);

                // Activa el período de invencibilidad
                StartCoroutine(ActivateInvincibility());

                // Detiene el empuje después de un breve período
                StartCoroutine(StopBounce());
            }
        }
    }
    /* public void beingAttacked(Vector2 direccion, int DMG) // Metodo para dañar al jugador
     {
         if (!isAttacked && !isInvincible) // Solo recibe daño si no está en período de invencibilidad
         {
             isAttacked = true;
             animator.SetBool("isAttacked", isAttacked);

             // Reduce la salud
             currentHealth -= DMG;
             UpdateLivesText(); // Actualiza el texto de las vidas

             // Empuja al personaje en la dirección del ataque
             Vector2 bounce = new Vector2(transform.position.x - direccion.x, 1).normalized;
             cuerpoRigido.AddForce(bounce * bounceStrength, ForceMode2D.Impulse);

             // Activa el período de invencibilidad
             StartCoroutine(ActivateInvincibility());

             // Detiene el empuje después de un breve período
             StartCoroutine(StopBounce());

             // Verifica si el personaje murió
             if (currentHealth <= 0)
             {
                 Die();
             }
         }
     }*/

    IEnumerator StopBounce()
    {
        yield return new WaitForSeconds(0.2f); // Espera un breve período
        cuerpoRigido.velocity = Vector2.zero; // Detiene el movimiento del Rigidbody2D
    }

    IEnumerator ActivateInvincibility()
    {
        isInvincible = true; // Activa la invencibilidad
        yield return new WaitForSeconds(invincibilityTime); // Espera el tiempo de invencibilidad
        isInvincible = false; // Desactiva la invencibilidad
        isAttacked = false; // Restablece el estado de ataque
        animator.SetBool("isAttacked", false); // Restablece la animación de daño
    }

    private void Die()
    {
        Debug.Log("¡Jugador derrotado!");
        animator.SetTrigger("Dead");// Reproduce la animación de muerte
        if (ScoreManager.Instance != null) 
        {
            ScoreManager.Instance.SaveTotalScore();
        }
        if (musicManager != null)
        {
            musicManager.StopAllSounds();
            musicManager.PlaySFX(musicManager.Death);
        }
        // Desactiva el movimiento y el control del personaje
        enabled = false;
        // Desactiva el collider (opcional)
        GetComponent<Collider2D>().enabled = false;

        // Desactiva el Rigidbody2D (opcional)
        GetComponent<Rigidbody2D>().simulated = false;

        StartCoroutine(LoadDeadScene());
        // Carga la escena de muerte
        //UnityEngine.SceneManagement.SceneManager.LoadScene("DeadScene");

        // Reinicia la escena después de un breve retraso
        //StartCoroutine(RestartLevel());
    }

    private IEnumerator LoadDeadScene()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos antes de cargar la escena de muerte
        UnityEngine.SceneManagement.SceneManager.LoadScene("DeadScene");
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos antes de reiniciar
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }


    private void UpdateLivesText()
    {
        if (Lives != null) // Asegúrate de que Lives no sea nulo
        {
            Lives.text = "Lives: " + currentHealth.ToString(); // Usa currentHealth en lugar de currentLives
        }
        else
        {
            Debug.LogWarning("El campo Lives no está asignado en el Inspector.");
        }
    }
}