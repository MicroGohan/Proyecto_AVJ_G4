using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Animator animator;
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
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
}
