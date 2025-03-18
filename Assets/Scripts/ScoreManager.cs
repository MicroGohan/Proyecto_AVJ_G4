using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText; // Referencia al texto TMP
    private int score = 0;     // Variable para almacenar el puntaje

    void Start()
    {
        // Inicializa el texto con el puntaje inicial
        UpdateScoreText();
    }

    void Update()
    {
        // Verifica si se presiona la tecla "P"
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddScore(1); // Aumenta el puntaje en 1
        }
    }

    // Método para aumentar el puntaje
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // Método para actualizar el texto del puntaje
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
