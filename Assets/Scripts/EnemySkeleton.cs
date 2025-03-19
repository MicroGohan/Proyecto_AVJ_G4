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

    [Header("Punto de Ataque")]
    public Transform attackPoint; // Crear un GameObject hijo y asignarlo
    public float attackRadius = 0.5f; // Radio del área de daño
    // Start is called before the first frame update
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
    }

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
        // Verifica si es momento de atacar
        if (Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown; // Establece el próximo momento de ataque
        }
    }

    private void Attack()
    {
        // Elige un ataque aleatorio
        int attackType = Random.Range(1, 4);
        switch (attackType)
        {
            case 1:
                animator.SetTrigger("Attack1");
                break;
            case 2:
                animator.SetTrigger("Attack2");
                break;
            case 3:
                animator.SetTrigger("Attack3");
                break;
        }

        // Ejecutar daño con delay
        StartCoroutine(ApplyDamageWithDelay(0.3f)); // Ajustar tiempo según animación
    }

    // Nuevo método para aplicar daño
    private IEnumerator ApplyDamageWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            Player
        );

        foreach (Collider2D playerCollider in hitPlayers)
        {
            CharacterController player = playerCollider.GetComponent<CharacterController>();
            if (player != null)
            {
                Vector2 attackDirection = (player.transform.position - transform.position).normalized;
                player.beingAttacked(attackDirection, attackDamage);
                Debug.Log("¡Golpe exitoso!");
            }
        }
    }



    public void TakeDamage(int damage)
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
    }

    private void Die()
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
    }

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
