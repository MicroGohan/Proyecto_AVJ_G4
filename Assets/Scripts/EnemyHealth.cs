using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Animator animator;
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    private bool isStunned = false;  // A�adir esta variable
    private bool isAttacking = false; // A�adir esta variable
    private Rigidbody2D rb;          // A�adir esta variable

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
            Debug.LogError("No se encontr� el componente EnemySkeleton");
        }
    }

    public void TakeDamage(int damage)
    {
        // No procesar da�o si est� muerto
        if (currentHealth <= 0) return;

        // Reduce la salud del esqueleto
        currentHealth -= damage;
        Debug.Log("Esqueleto recibi� " + damage + " de da�o. Salud restante: " + currentHealth);

        // Si muri�, manejar la muerte inmediatamente
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // Interrumpe cualquier ataque o acci�n actual
        StopAllCoroutines();
        isStunned = true;
        isAttacking = false;

        // Resetea todos los estados y triggers
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.SetBool("isRunning", false);

        // Reproduce la animaci�n de da�o
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
        // Aseg�rate de que solo se ejecute una vez
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

        // Activar animaci�n de muerte
        animator.SetTrigger("Dead");

        // A�adir puntos
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

        // Destruir despu�s de la animaci�n
        Destroy(gameObject, 1f);
    }

    /*private void Die()
    {
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("�No hay ScoreManager en la escena!");
            return;
        }

        ScoreManager.Instance.AddScore(10);
        Debug.Log("Puntos asignados"); // A�adir este debug

        Debug.Log("�Esqueleto derrotado!");
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
        // Destruye el objeto despu�s de un breve retraso (opcional)
        Destroy(gameObject, 0.5f);
    }*/
}
