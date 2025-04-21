using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3; // Vidas m�ximas del jugador
    public int currentLives; // Vidas actuales del jugador
    public TMP_Text livesText; // Referencia al texto TMP de las vidas
    private MusicManager musicManager;
    void Start()
    {
        currentLives = maxLives; // Inicializa las vidas al m�ximo
        UpdateLivesText(); // Actualiza el texto de las vidas
        musicManager = FindObjectOfType<MusicManager>();
    }
    
    // M�todo para recibir da�o
    public void TakeDamage(int damage)
    {
        currentLives -= damage; // Reduce las vidas
        Debug.Log("�Jugador recibi� da�o! Vidas restantes: " + currentLives);
        /*if (musicManager != null)
        {
            musicManager.PlaySFX(musicManager.Hurt);
        }*/
        // Actualiza el texto de las vidas
        UpdateLivesText();

        // Verifica si el jugador muri�
        if (currentLives <= 0)
        {
            Die();
        }
    }

    // M�todo para actualizar el texto de las vidas
    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + currentLives.ToString();
    }

    // M�todo para manejar la muerte del jugador
    void Die()
    {
        Debug.Log("�Jugador derrotado2!");
        if (musicManager != null)
        {
            musicManager.PlaySFX(musicManager.Death);
        }
        // Aqu� puedes agregar l�gica para reiniciar el nivel o mostrar un Game Over
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}