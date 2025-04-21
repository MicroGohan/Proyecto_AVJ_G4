using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3; // Vidas máximas del jugador
    public int currentLives; // Vidas actuales del jugador
    public TMP_Text livesText; // Referencia al texto TMP de las vidas
    private MusicManager musicManager;
    void Start()
    {
        currentLives = maxLives; // Inicializa las vidas al máximo
        UpdateLivesText(); // Actualiza el texto de las vidas
        musicManager = FindObjectOfType<MusicManager>();
    }
    
    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        currentLives -= damage; // Reduce las vidas
        Debug.Log("¡Jugador recibió daño! Vidas restantes: " + currentLives);
        /*if (musicManager != null)
        {
            musicManager.PlaySFX(musicManager.Hurt);
        }*/
        // Actualiza el texto de las vidas
        UpdateLivesText();

        // Verifica si el jugador murió
        if (currentLives <= 0)
        {
            Die();
        }
    }

    // Método para actualizar el texto de las vidas
    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + currentLives.ToString();
    }

    // Método para manejar la muerte del jugador
    void Die()
    {
        Debug.Log("¡Jugador derrotado2!");
        if (musicManager != null)
        {
            musicManager.PlaySFX(musicManager.Death);
        }
        // Aquí puedes agregar lógica para reiniciar el nivel o mostrar un Game Over
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}