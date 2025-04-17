using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeadSceneManager : MonoBehaviour
{
    [Tooltip("Referencia al texto TMP para mostrar el puntaje final.")]
    public TMP_Text scoreText;

    void Start()
    {
        DisplayScores();
    }

    void DisplayScores()
    {
        Debug.Log("DisplayScores llamado");

        if (scoreText == null)
        {
            Debug.LogError("¡scoreText no está asignado en el Inspector!");
            return;
        }

        int finalScore = 0;

        // Intenta obtener el puntaje del ScoreManager
        if (ScoreManager.Instance != null)
        {
            Debug.Log("ScoreManager.Instance encontrado");
            finalScore = ScoreManager.Instance.GetTotalScore();
            Debug.Log($"Puntaje obtenido del ScoreManager: {finalScore}");
        }
        else
        {
            Debug.Log("ScoreManager.Instance es null, usando PlayerPrefs");
            finalScore = PlayerPrefs.GetInt("TotalScore", 0);
            Debug.Log($"Puntaje obtenido de PlayerPrefs: {finalScore}");
        }

        // Actualiza el texto
        try
        {
            Debug.Log($"Intentando actualizar scoreText con valor: {finalScore}");
            scoreText.text = $"Total Score: {finalScore}";
            Debug.Log($"Texto establecido en scoreText: {scoreText.text}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al actualizar el texto: {e.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
