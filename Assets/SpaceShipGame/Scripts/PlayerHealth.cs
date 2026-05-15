using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    [Header("Vidas")]
    public int maxLives = 3;

    [Header("Invencibilidad tras recibir daño")]
    public float invincibleDuration = 1.5f;

    [Header("Corazones UI")]
    public Image[] heartImages = new Image[3]; // arrastra los 3 en el Inspector

    private EnemyController enemy;

    // Estado
    private int currentLives;
    private bool isInvincible = false;
    private float invincibleTimer = 0f;


    public static PlayerHealth Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        currentLives = maxLives;
    }

    void Start()
    {
        UpdateHeartsUI();
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
                isInvincible = false;
        }
    }
    

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
                heartImages[i].color = i < currentLives
                    ? Color.white                          // vida activa → visible
                    : new Color(1f, 1f, 1f, 0.15f);       // vida perdida → transparente
        }
    }


    // ── Daño y muerte ──────────────────────────────────────────────────────
    public void TakeDamage(int amount = 1)
    {
        if (isInvincible) return;

        currentLives -= amount;
        Debug.Log($"Jugador golpeado. Vidas: {currentLives}/{maxLives}");

        UpdateHeartsUI();

        if (currentLives <= 0)
            Die();
        else
            StartInvincibility();
    }

    void StartInvincibility()
    {
        isInvincible = true;
        invincibleTimer = invincibleDuration;
    }

    public void Die()
    {

        Debug.Log("Game Over");

        if (Application.CanStreamedLevelBeLoaded("GameOver"))
            SceneManager.LoadScene("GameOver");
        else
        {
            Debug.LogWarning("No se encontró la escena 'GameOver'.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet") || other.CompareTag("Enemy"))
        {
            TakeDamage(1);
            if (!(other.CompareTag("Enemy") && enemy.maxHealth > 5))
            {
                
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            TakeDamage(1);
    }

    public int GetLives() => currentLives;
}