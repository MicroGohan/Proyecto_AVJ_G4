using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private void Start()
    {
        // Asegurarse de que el portal empiece desactivado
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"OnTriggerEnter2D ejecutado. GameObject: {gameObject.name}");
        Debug.Log($"Colisión detectada con: {other.name}, Tag: {other.tag}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador entró al portal. Cargando siguiente nivel...");
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnLevelComplete(); // Guardar puntaje antes de cambiar nivel
            }
            SceneManager.LoadScene("Level_2");
        }
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(0.5f);

        try
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnLevelComplete();
            }
            SceneManager.LoadScene("Level_2");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al cargar Level_2: {e.Message}");
        }
    }
}
