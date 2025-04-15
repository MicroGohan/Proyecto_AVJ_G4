using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText; // Referencia al texto TMP
    private int score = 0;     // Variable para almacenar el puntaje

    public static ScoreManager Instance;

    void Awake()
    {
        // Configurar patr�n Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
            AddScore(10); // Aumenta el puntaje en 1
        }
    }

    // M�todo para aumentar el puntaje
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        Debug.Log($"Puntos sumados: {points} | Total: {score}");
    }

    // M�todo para actualizar el texto del puntaje
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    // Propiedad p�blica para acceder al puntaje actual
    public int CurrentScore
    {
        get { return score; }
    }
}
