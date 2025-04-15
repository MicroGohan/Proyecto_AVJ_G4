using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeadSceneManager : MonoBehaviour
{
    [Tooltip("Referencia al texto TMP para mostrar el puntaje.")]
    public TMP_Text scoreText;

    void Start()
    {
        // Asegúrate de que ScoreManager.Instance no sea nulo
        if (ScoreManager.Instance != null && scoreText != null)
        {
            // Obtén el puntaje actual y actualiza el texto
            scoreText.text = "Final Score: " + ScoreManager.Instance.CurrentScore.ToString();
        }
        else
        {
            Debug.LogWarning("ScoreManager o scoreText no están configurados correctamente.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
