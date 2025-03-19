using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackArea : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Versión mejorada con chequeo de componentes
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Daño aplicado: {damage} a {other.name}");
            }
        }
    }
}
