using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VictoryManager : MonoBehaviour
{

    public GameObject victoryPanel;     
    public Button     restartButton;    

    public string gameSceneName = "SolarSystem";


    public static VictoryManager Instance { get; private set; }

    // Contador de enemigos
    private int totalEnemiesKilled = 0;
    private bool bossKilled        = false;
    private bool victoryTriggered  = false;

    void Awake()
    {
        Instance = this;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    void Start()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(Restart);
    }
    public void OnEnemyKilled()
    {
        totalEnemiesKilled++;
        CheckVictory();
    }

    public void OnBossKilled()
    {
        bossKilled = true;
        CheckVictory();
    }

    void CheckVictory()
    {
        if (victoryTriggered) return;

        int enemiesNeeded = EnemySpawner.Instance != null
            ? EnemySpawner.Instance.enemiesBeforeBoss
            : 0;

        if (bossKilled)
            TriggerVictory();
    }

    void TriggerVictory()
    {
        victoryTriggered = true;
        Debug.Log("¡VICTORIA!");

        // Mostrar panel
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        // Pausar el juego
        Time.timeScale = 0f;

        // Mostrar cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f; // reanudar tiempo antes de cambiar escena
        SceneManager.LoadScene(gameSceneName);
    }
    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
