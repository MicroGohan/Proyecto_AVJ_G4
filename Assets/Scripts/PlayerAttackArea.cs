using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackArea : MonoBehaviour
{
    public int damage = 1;
    private Collider2D attackCollider;
    private bool isActive = false;
    private MusicManager musicManager;

    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    private void Awake()
    {
        attackCollider = GetComponent<PolygonCollider2D>();
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
        else
        {
            Debug.LogError("No se encontró PolygonCollider2D en " + gameObject.name);
        }
    }
    public void EnableArea()
    {
        if (attackCollider != null && !isActive)
        {
            isActive = true;
            attackCollider.enabled = true;
            Debug.Log($"Área de ataque activada en {Time.time}");

            // Forzar la desactivación después de un tiempo por seguridad
            StartCoroutine(ForceDisableAfterDelay(0.5f));
        }
    }

    public void DisableArea()
    {
        if (attackCollider != null && isActive)
        {
            isActive = false;
            attackCollider.enabled = false;
            Debug.Log($"Área de ataque desactivada en {Time.time}");
        }
    }
    private IEnumerator ForceDisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isActive)
        {
            Debug.LogWarning("Forzando desactivación del área de ataque por timeout");
            DisableArea();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return; // Extra verificación

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Daño aplicado: {damage} a {other.name}");
            }
        }
        if (musicManager != null)
        {
            musicManager.PlaySFX(musicManager.Attack);
        }
    }
    private void OnDisable()
    {
        // Asegurarse de que el área se desactive si se desactiva el GameObject
        DisableArea();
    }

}
