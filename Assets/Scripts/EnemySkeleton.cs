using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : EnemyBehaviour
{
    public LayerMask Player; // Asigna la capa "Player" en el Inspector
    public float speed;
    public float chaseDistance;
    public float stopDistance;
    public float attackDistance; // Distancia a la que el enemigo ataca
    public float attackCooldown; // Tiempo entre ataques
    public int attackDamage; // Daño que hace el enemigo
    public int maxHealth = 3; // Salud máxima del esqueleto
    private int currentHealth; // Salud actual del esqueleto
    public GameObject target;
    private float targetDistance;
    private float nextAttackTime = 0f; // Tiempo para el próximo ataque
    Animator animator;
    private Rigidbody2D rb;
    private bool isStunned = false;
    private bool isAttacking = false;// Indica si el enemigo está aturdido
    [Header("Punto de Ataque")]
    public Transform attackPoint; // Crear un GameObject hijo y asignarlo
    public float attackRadius = 1f; // Radio del área de daño
    // Start is called before the first frame update
    private float attack1Duration = 1.0f;
    private float attack2Duration = 1.0f;
    private float attack3Duration = 1.0f;
    private float currentAttackDuration; // Añadir esta variable
    private bool isPlayingAnimation = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth; // Inicializa la salud al máximo
        target = GameObject.FindGameObjectWithTag("Player");

        if (target == null)
            Debug.LogError("¡No se encontró al jugador con tag 'Player'!");
    }
    void Update()
    {
        if (target == null || isStunned) return; // No hacer nada si está stunneado

        targetDistance = Vector2.Distance(transform.position, target.transform.position);

        // Si el jugador está dentro del rango de persecución pero fuera del rango de ataque
        if (targetDistance < chaseDistance && targetDistance > stopDistance)
        {
            ChasePlayer();
        }
        // Si el jugador está dentro del rango de ataque
        else if (targetDistance <= attackDistance)
        {
            StopChasePlayer();
            TryToAttack();
        }
        // Si el jugador está fuera del rango de persecución
        else
        {
            StopChasePlayer();
            Idle();
        }
    }
    /*void Update()
    {
        if (target == null) return;

        targetDistance = Vector2.Distance(transform.position, target.transform.position);
        
        // Si el jugador está dentro del rango de persecución pero fuera del rango de ataque
        if (targetDistance < chaseDistance && targetDistance > stopDistance)
        {
            ChasePlayer();
        }
        // Si el jugador está dentro del rango de ataque
        else if (targetDistance <= attackDistance)
        {
            StopChasePlayer();
            TryToAttack();
        }
        // Si el jugador está fuera del rango de persecución
        else
        {
            StopChasePlayer();
            Idle();
        }
    }*/

    private void ChasePlayer()
    {
        if(transform.position.x < target.transform.position.x)
        
            GetComponent<SpriteRenderer>().flipX = false;
        else
            GetComponent<SpriteRenderer>().flipX = true;
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        animator.SetBool("isRunning", true);
    }

    private void StopChasePlayer()
    {
        animator.SetBool("isRunning", false);
    }

    private void Idle()
    {
        animator.Play("IdleSkeleton");
    }

    private void TryToAttack()
    {
        // No atacar si está muerto, aturdido o en medio de una animación
        if (currentHealth <= 0 || isStunned || isPlayingAnimation) return;

        if (Time.time >= nextAttackTime && !isAttacking)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown; // Reduce este valor en el Inspector (por ejemplo, a 1.5f)
        }
    }

    private void Attack()
    {
        if (currentHealth <= 0 || isStunned) return;

        isAttacking = true;
        isPlayingAnimation = true;

        // Asegura que el enemigo mire al jugador
        if (target != null)
        {
            GetComponent<SpriteRenderer>().flipX = (target.transform.position.x < transform.position.x);
        }

        int attackType = Random.Range(1, 4);
        switch (attackType)
        {
            case 1:
                animator.SetTrigger("Attack1");
                currentAttackDuration = attack1Duration;
                break;
            case 2:
                animator.SetTrigger("Attack2");
                currentAttackDuration = attack2Duration;
                break;
            case 3:
                animator.SetTrigger("Attack3");
                currentAttackDuration = attack3Duration;
                break;
        }

        /*StartCoroutine(ResetAttackState(0.5f)); // Asegura que isAttacking se resetee
        StartCoroutine(ApplyDamageWithDelay(0.7f));
    }*/
        StartCoroutine(ApplyDamageWithDelay(currentAttackDuration * 0.75f));
        StartCoroutine(FinishAttackAnimation(currentAttackDuration));
    }
    private IEnumerator FinishAttackAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);
        isPlayingAnimation = false;
        isAttacking = false;
    }
    // Llamar en el frame donde la animación muestra el golpe

    // Agregar este método para asegurar que el estado de ataque se resetee
    private IEnumerator ResetAttackState(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }
    // Y agregar este método para finalizar el ataque
    public void OnAttackEnd() // Llamar esto desde un Animation Event
    {
        isAttacking = false;
    }

    // Nuevo método para aplicar daño
    private IEnumerator ApplyDamageWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Solo aplica daño si el enemigo sigue vivo y no está aturdido
        if (currentHealth <= 0 || isStunned) yield break;

        // Busca específicamente colisiones con el Box Collider del jugador
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            Player
        );

        foreach (Collider2D playerCollider in hitPlayers)
        {
            // Verifica específicamente si es un BoxCollider2D
            if (playerCollider is BoxCollider2D)
            {
                CharacterController player = playerCollider.GetComponent<CharacterController>();
                if (player != null)
                {
                    Vector2 attackDirection = (player.transform.position - transform.position).normalized;
                    player.beingAttacked(attackDirection, attackDamage);
                    Debug.Log("¡Golpe exitoso al Box Collider del jugador!");
                }
            }
        }
    }

    /* public void TakeDamage(int damage)
     {
         // Reduce la salud del esqueleto
         currentHealth -= damage;
         Debug.Log("Esqueleto recibió " + damage + " de daño. Salud restante: " + currentHealth);

         // Reproduce la animación de daño
         if (animator != null)
         {
             animator.SetTrigger("Hurt");
         }
         else
         {
             Debug.LogWarning("Animator no está asignado en el esqueleto.");
         }

         // Verifica si el esqueleto murió
         if (currentHealth <= 0)
         {
             Die();
         }
     }*/
    public void TakeDamage(int damage)
    {
        // No procesar daño si está muerto
        if (currentHealth <= 0) return;

        // Reduce la salud del esqueleto
        currentHealth -= damage;
        Debug.Log("Esqueleto recibió " + damage + " de daño. Salud restante: " + currentHealth);

        // Si murió, manejar la muerte inmediatamente
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // Interrumpe cualquier ataque o acción actual
        StopAllCoroutines();
        isStunned = true;
        isAttacking = false;
        isPlayingAnimation = false;

        // Resetea todos los estados y triggers
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.SetBool("isRunning", false);

        // Reproduce la animación de daño
        animator.SetTrigger("Hurt");
        StartCoroutine(RecoverFromStun(0.5f));
    }

    private IEnumerator RecoverFromStun(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }
    private void Die()
    {
        // Asegúrate de que solo se ejecute una vez
        if (!enabled) return;

        // Detener todas las corrutinas y estados
        StopAllCoroutines();
        isStunned = true;
        isAttacking = false;

        // Resetear todos los triggers y estados
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("Hurt");
        animator.SetBool("isRunning", false);

        // Activar animación de muerte
        animator.SetTrigger("Dead");

        // Añadir puntos
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(10);
            Debug.Log("Puntos asignados");
        }

        // Desactivar comportamientos
        enabled = false;

        // Desactivar colisiones
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        if (rb != null)
        {
            rb.simulated = false;
        }
        // Cancelar cualquier ataque pendiente
        isAttacking = false;
        isPlayingAnimation = false;

        // Destruir después de la animación
        Destroy(gameObject, 2f);
    }
    /*private void Die()
    {
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("¡No hay ScoreManager en la escena!");
            return;
        }

        ScoreManager.Instance.AddScore(10);
        Debug.Log("Puntos asignados"); // Añadir este debug

        Debug.Log("¡Esqueleto derrotado!");
        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }

        // Desactiva el comportamiento del esqueleto
        enabled = false;

        // Desactiva el collider (opcional)
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Desactiva el Rigidbody2D (opcional)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
        }
        // Destruye el objeto después de un breve retraso (opcional)
        Destroy(gameObject, 2f);
    }*/

    /*private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccionDMG = new Vector2(transform.position.x, 0);
            collision.gameObject.GetComponent<CharacterController>().beingAttacked(direccionDMG, 1);
        }

    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Mejor cálculo de dirección
            Vector2 direccionDMG = (collision.transform.position - transform.position).normalized;

            // Asegurar que el jugador tenga el componente
            CharacterController player = collision.gameObject.GetComponent<CharacterController>();
            if (player != null)
            {
                player.beingAttacked(direccionDMG, 1);
            }
        }
    }
}
