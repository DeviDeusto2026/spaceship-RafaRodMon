using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnRadius = 2f;

    public GameObject bossPrefab;
    public int enemiesBeforeBoss = 10;
    public float bossSpawnDistance = 40f;  

    public TextMeshProUGUI counterText;  

    private int enemiesSpawned = 0;
    private bool bossSpawned = false;
    private Transform player;

    public static EnemySpawner Instance { get; private set; }

    void Awake() { Instance = this; }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        UpdateCounterUI();
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (bossSpawned || enemyPrefab == null) return;

        Vector3 offset = Random.insideUnitSphere * spawnRadius;
        offset.z = 0f;
        Vector3 spawnPos = transform.position + offset;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemiesSpawned++;

        UpdateCounterUI();

        if (enemiesSpawned >= enemiesBeforeBoss)
            SpawnBoss();
    }

    void SpawnBoss()
    {
        if (bossPrefab == null) return;
        bossSpawned = true;
        CancelInvoke(nameof(SpawnEnemy));

        // Calcular posición lejos del jugador
        Vector3 spawnPos;
        if (player != null)
        {
           spawnPos = new Vector3(
            player.position.x + bossSpawnDistance,
            player.position.y,                       
            player.position.z + bossSpawnDistance   
       );
        }
        else
        {
            spawnPos = transform.position;
        }

        Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"¡Boss spawneado a {bossSpawnDistance} unidades del jugador!");

        // Actualizar HUD al spawnear boss
        if (counterText != null)
            counterText.text = "⚠ BOSS ⚠";
    }

    void UpdateCounterUI()
    {
        if (counterText == null) return;
        int remaining = enemiesBeforeBoss - enemiesSpawned;
        counterText.text = remaining > 0
            ? $"Enemigos para el Boss: {remaining}"
            : "¡Boss en camino!";
    }

    public void ResumeSpawning()
    {
        bossSpawned = false;
        enemiesSpawned = 0;
        UpdateCounterUI();
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }
}