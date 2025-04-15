using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // M�todo para el bot�n "Iniciar Juego"
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    // M�todo para el bot�n "Salir"
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // M�todo para el bot�n "Retry"
    public void Retry()
    {
        // Recarga la escena actual
        SceneManager.LoadScene("MainScene");
    }
}
