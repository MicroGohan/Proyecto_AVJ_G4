using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackArea : MonoBehaviour
{
    public int damage = 1;
    private Collider2D attackCollider;

    private void Awake()
    {
        attackCollider = GetComponent<PolygonCollider2D>();
        if (attackCollider != null)
        {
            attackCollider.enabled = false; // Desactiva el collider al inicio
        }
    }
    public void EnableArea()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true; // Activa el collider
        }
    }
    public void DisableArea()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false; // Desactiva el collider
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Daño aplicado: {damage} a {other.name}");
            }
        }
    }
    
}
