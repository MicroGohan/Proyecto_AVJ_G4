using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Animator animator;
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    private bool isStunned = false;  // Añadir esta variable
    private bool isAttacking = false; // Añadir esta variable
    private Rigidbody2D rb;          // Añadir esta variable
    [SerializeField] private float hurtAnimationDuration = 0.20f; // Duración de la animación de daño
    [SerializeField] private float stunDuration = 1.0f; // Tiempo de aturdimiento
    private bool isHurt = false;

    private EnemySkeleton enemySkeleton;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>(); // Inicializar rb
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemySkeleton = GetComponent<EnemySkeleton>(); // Obtener referencia
        if (enemySkeleton == null)
        {
            Debug.LogError("No se encontró el componente EnemySkeleton");
        }
    }

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

        // Marca que está siendo herido
        isHurt = true;
        isStunned = true;
        isAttacking = false;

        // Interrumpe cualquier ataque o acción actual
        StopAllCoroutines();

        // Resetea el estado de movimiento
        animator.SetBool("isRunning", false);

        // Reproduce la animación de daño
        animator.SetTrigger("Hurt");

        // Inicia las corrutinas de recuperación
        StartCoroutine(HurtRecovery());
        StartCoroutine(RecoverFromStun(stunDuration));
    }

    private IEnumerator HurtRecovery()
    {
        // Espera a que termine la animación de daño
        yield return new WaitForSeconds(hurtAnimationDuration);

        // Resetea los triggers solo después de que la animación de daño termine
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("Hurt");

        isHurt = false;
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
        isHurt = true;

        // Resetear estados de movimiento
        animator.SetBool("isRunning", false);

        // Activar animación de muerte
        animator.SetTrigger("Dead");

        // Añadir puntos
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(10);
            Debug.Log("Puntos asignados");
        }

        enabled = false;
        if (enemySkeleton != null)
        {
            enemySkeleton.enabled = false;
        }

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
        StartCoroutine(DestroyAfterAnimation());
        // Destruir después de la animación
        Destroy(gameObject, 1f);
    }
    private IEnumerator DestroyAfterAnimation()
    {
        // Esperar a que la animación de muerte termine
        yield return new WaitForSeconds(1f);

        // Desactivar el GameObject completo antes de destruirlo
        gameObject.SetActive(false);

        // Destruir el objeto
        Destroy(gameObject);
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
        Destroy(gameObject, 0.5f);
    }*/
}
