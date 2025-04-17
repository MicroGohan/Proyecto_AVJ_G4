using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText; // Referencia al texto TMP
    public GameObject portal;
    private int score = 0;
    private int currentLevelScore = 0;
    private int totalAccumulatedScore = 0;  // Puntaje total acumulado
    private int highScore = 0;
    private bool isLevel2 = false;

    public static ScoreManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Solo cargamos el total si NO estamos en la primera escena
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name != "Level_1") // o el nombre de tu primera escena
            {
                totalAccumulatedScore = PlayerPrefs.GetInt("TotalScore", 0);
            }
            else
            {
                ResetScores(); // Reiniciamos los puntajes al iniciar nuevo juego
            }
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void ResetScores()
    {
        currentLevelScore = 0;
        totalAccumulatedScore = 0;
        PlayerPrefs.SetInt("TotalScore", 0);
        PlayerPrefs.Save();
        UpdateScoreText();
        Debug.Log("Scores reiniciados");
    }
    void Start()
    {
        // Inicializa el texto con el puntaje inicial
        UpdateScoreText();

        // Asegúrate de que el portal esté desactivado al inicio
        if (portal != null)
        {
            portal.SetActive(false);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level_2")
        {
            Debug.Log($"Cambiando a Level_2. Guardando puntaje del nivel anterior: {currentLevelScore}");
            isLevel2 = true;
        }
        // Verifica si estamos en el Level_2
        isLevel2 = scene.name == "Level_2";
        // Solo reiniciamos el score del nivel actual
        currentLevelScore = 0;
        //score = 0;
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
        
        //score += points;
        currentLevelScore += points;
        totalAccumulatedScore += points;
        UpdateScoreText();
        Debug.Log($"Puntos sumados: {points} | Nivel: {currentLevelScore} | Total: {totalAccumulatedScore}");
        
        // Si estamos en el nivel 2 y alcanzamos 40 puntos
        if (isLevel2 && currentLevelScore >= 40)
        {
            GoToDeathScene();
        }
        // Si estamos en el nivel 1 y alcanzamos 50 puntos
        else if (!isLevel2 && currentLevelScore >= 50)
        {
            OpenPortal();
        }
    }
    
    // Método para actualizar el texto del puntaje
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentLevelScore}";
        }
    }

    // Método para abrir el portal
    private void OpenPortal()
    {
        Debug.Log("¡Portal abierto!");
        if (portal != null)
        {
            portal.SetActive(true); // Activa el portal
        }
    }
    
    // Llamar cuando se complete un nivel
    public void OnLevelComplete()
    {
        SaveTotalScore();
    }

    // Propiedad pública para acceder al puntaje actual
    public int CurrentScore
    {
        get { return score; }
    }

    
    private void GoToDeathScene()
    {
        Debug.Log($"¡Yendo a la escena de muerte! Puntaje total: {totalAccumulatedScore}");
        // Guarda el puntaje antes de cambiar de escena
        SaveTotalScore();
        // Asegúrate de que la instancia no se destruya
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("DeadScene");
    }

    public void SaveTotalScore()
    {
        Debug.Log($"Guardando puntaje total: {totalAccumulatedScore}");
        PlayerPrefs.SetInt("TotalScore", totalAccumulatedScore);
        PlayerPrefs.Save();
    }

    public int GetTotalScore()
    {
        Debug.Log($"Obteniendo puntaje total: {totalAccumulatedScore}");
        return totalAccumulatedScore;
    }
    public int HighScore
    {
        get { return highScore; }
    }

    private void LoadNextScene()
    {
        Debug.Log("Cargando la siguiente escena...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
