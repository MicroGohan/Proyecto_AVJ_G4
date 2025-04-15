using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Método para el botón "Iniciar Juego"
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    // Método para el botón "Salir"
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // Método para el botón "Retry"
    public void Retry()
    {
        // Recarga la escena actual
        SceneManager.LoadScene("MainScene");
    }
}
