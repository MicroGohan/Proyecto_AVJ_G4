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
            Debug.LogError("No se encontr� PolygonCollider2D en " + gameObject.name);
        }
    }
    public void EnableArea()
    {
        if (attackCollider != null && !isActive)
        {
            isActive = true;
            attackCollider.enabled = true;
            Debug.Log($"�rea de ataque activada en {Time.time}");

            // Forzar la desactivaci�n despu�s de un tiempo por seguridad
            StartCoroutine(ForceDisableAfterDelay(0.5f));
        }
    }

    public void DisableArea()
    {
        if (attackCollider != null && isActive)
        {
            isActive = false;
            attackCollider.enabled = false;
            Debug.Log($"�rea de ataque desactivada en {Time.time}");
        }
    }
    private IEnumerator ForceDisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isActive)
        {
            Debug.LogWarning("Forzando desactivaci�n del �rea de ataque por timeout");
            DisableArea();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return; // Extra verificaci�n

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Da�o aplicado: {damage} a {other.name}");
            }
        }
        if (musicManager != null)
        {
            musicManager.PlaySFX(musicManager.Attack);
        }
    }
    private void OnDisable()
    {
        // Asegurarse de que el �rea se desactive si se desactiva el GameObject
        DisableArea();
    }

}
