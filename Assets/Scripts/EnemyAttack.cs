using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 10; // Daño que hace el enemigo
    public float attackRange = 2f; // Rango del ataque
    public float attackCooldown = 2f; // Tiempo entre ataques
    public Transform attackPoint; // Punto desde donde se detecta el ataque
    public LayerMask playerLayer; // Capa del jugador

    private Animator animator;
    private float nextAttackTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Verifica si es momento de atacar
        if (Time.time >= nextAttackTime)
        {
            if (IsPlayerInRange())
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown; // Establece el próximo momento de ataque
            }
        }
    }

    bool IsPlayerInRange()
    {
        // Detecta si el jugador está dentro del rango de ataque
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        return hitPlayers.Length > 0;
    }

    void Attack()
    {
        // Reproduce la animación de ataque
        animator.SetTrigger("IsAttacking");

        // Detecta al jugador en el rango de ataque
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        // Aplica daño a todos los jugadores golpeados
        foreach (Collider2D player in hitPlayers)
        {
            if (player.GetComponent<PlayerHealth>() != null)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
                Debug.Log("¡Enemigo atacó al jugador! Daño: " + attackDamage);
            }
        }
    }

    // Dibuja el rango de ataque en el editor (solo para debug)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
