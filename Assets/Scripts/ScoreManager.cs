using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText; // Referencia al texto TMP
    private int score = 0;     // Variable para almacenar el puntaje

    public static ScoreManager Instance;

    void Awake()
    {
        // Configurar patrón Singleton
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

    // Método para aumentar el puntaje
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        Debug.Log($"Puntos sumados: {points} | Total: {score}");

        // Verifica si el puntaje ha alcanzado 40
        if (score >= 40)
        {
            LoadNextScene();
        }
    }

    // Método para actualizar el texto del puntaje
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    // Propiedad pública para acceder al puntaje actual
    public int CurrentScore
    {
        get { return score; }
    }

    private void LoadNextScene()
    {
        Debug.Log("Cargando la siguiente escena...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
