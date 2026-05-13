using UnityEngine;

/// <summary>
/// Spawner de enemigos — instancia en la posición del propio GameObject.
/// Coloca este GameObject donde quieras que aparezcan los enemigos.
/// Para múltiples puntos de spawn, crea varios GameObjects con este script.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemigo normal")]
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;

    [Header("Dispersión aleatoria alrededor del spawner")]
    public float spawnRadius = 2f;   // 0 = exactamente en el punto, >0 = con dispersión

    [Header("Boss")]
    public GameObject bossPrefab;
    public int enemiesBeforeBoss = 10;

    private int enemiesSpawned = 0;
    private bool bossSpawned = false;

    public static EnemySpawner Instance { get; private set; }

    void Awake() { Instance = this; }

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (bossSpawned || enemyPrefab == null) return;

        // Spawn en la posición del spawner + dispersión aleatoria
        Vector3 offset = Random.insideUnitSphere * spawnRadius;
        offset.z = 0f; // mantener en el mismo plano si lo necesitas
        Vector3 spawnPos = transform.position + offset;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemiesSpawned++;

        if (enemiesSpawned >= enemiesBeforeBoss)
            SpawnBoss();
    }

    void SpawnBoss()
    {
        if (bossPrefab == null) return;
        bossSpawned = true;
        CancelInvoke(nameof(SpawnEnemy));
        Instantiate(bossPrefab, transform.position, Quaternion.identity);
        Debug.Log("¡Boss spawneado!");
    }

    public void ResumeSpawning()
    {
        bossSpawned = false;
        enemiesSpawned = 0;
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }
}