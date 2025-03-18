using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : EnemyBehaviour
{

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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth; // Inicializa la salud al máximo
    }

    void Update()
    {
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

        // Elige un ataque aleatorio (1, 2 o 3)
        int attackType = Random.Range(1, 4); // Random.Range(1, 4) genera un número entre 1 y 3

        // Reproduce la animación de ataque correspondiente
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

        // Detecta si el jugador está dentro del rango de ataque
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackDistance, LayerMask.GetMask("Player"));
        foreach (Collider2D player in hitPlayers)
        {
            if (player.CompareTag("Player"))
            {
                // Aplica daño al jugador
                player.GetComponent<CharacterController>().beingAttacked(Vector2.zero, attackDamage);
                Debug.Log("¡Enemigo atacó al jugador! Daño: " + attackDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // Si ya está muerto, no hace nada

        // Reduce la salud
        currentHealth -= damage;
        Debug.Log("Esqueleto recibió " + damage + " de daño. Salud restante: " + currentHealth);

        // Reproduce la animación de daño
        animator.SetTrigger("Hurt");

        // Si la salud llega a 0, muere
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Reproduce la animación de muerte
        animator.SetTrigger("Dead");

        // Desactiva el comportamiento del enemigo
        enabled = false;

        // Desactiva el collider (opcional)
        GetComponent<Collider2D>().enabled = false;

        // Desactiva el Rigidbody2D (opcional)
        GetComponent<Rigidbody2D>().simulated = false;

        Debug.Log("¡Esqueleto derrotado!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccionDMG = new Vector2(transform.position.x, 0);
            collision.gameObject.GetComponent<CharacterController>().beingAttacked(direccionDMG, 1);
        }

    }
}
