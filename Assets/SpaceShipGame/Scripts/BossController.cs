using UnityEngine;

/// <summary>
/// Boss — persigue al jugador lentamente, dispara hacia él, movimiento sinusoidal.
/// </summary>
public class BossController : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 15;

    [Header("Movimiento — persecución")]
    public float chaseSpeed = 2f;      // velocidad hacia el jugador
    public float stopDistance = 8f;      // distancia a la que se detiene

    [Header("Movimiento — sinusoidal")]
    public float sinAmplitude = 2f;
    public float sinFrequency = 1f;

    [Header("Disparo")]
    public GameObject bossBulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;

    [Header("Spawn de minions al recibir daño")]
    public GameObject minionPrefab;
    public int minionsPerHit = 1;

    [Header("Efectos")]
    public GameObject deathEffect;
    public GameObject hitEffect;

    private int currentHealth;
    private float timeAlive = 0f;
    private float nextFireTime = 0f;
    private Transform player;
    private bool isDead = false;

    // Dirección perpendicular al jugador para el seno (se recalcula al iniciar)
    private Vector3 sinAxis;

    void Start()
    {
        currentHealth = maxHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("BossController: no se encontró objeto con tag 'Player'");

        // Eje perpendicular para la oscilación (arriba en world space)
        sinAxis = Vector3.up;

        // Asegurarse de tener collider
        if (GetComponent<Collider>() == null)
        {
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            sc.radius = 2f;
        }
        else
        {
            GetComponent<Collider>().isTrigger = true;
        }
    }

    void Update()
    {
        if (isDead || player == null) return;
        timeAlive += Time.deltaTime;

        ChasePlayer();
        ShootAtPlayer();
    }

    void ChasePlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // Solo avanza si está lejos del jugador
        if (dist > stopDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            Vector3 sinWave = sinAxis * Mathf.Sin(timeAlive * sinFrequency) * sinAmplitude;
            Vector3 movement = (dir + sinWave) * chaseSpeed * Time.deltaTime;
            transform.position += movement;
        }
        else
        {
            // Dentro del rango: solo oscila
            transform.position += sinAxis * Mathf.Sin(timeAlive * sinFrequency)
                                          * sinAmplitude * Time.deltaTime;
        }

        // Siempre mira al jugador
        transform.LookAt(player.position);
    }

    void ShootAtPlayer()
    {
        if (bossBulletPrefab == null) return;
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        Transform fp = firePoint != null ? firePoint : transform;
        Vector3 dir = (player.position - fp.position).normalized;

        GameObject b = Instantiate(bossBulletPrefab, fp.position, Quaternion.identity);
        BossBullet bb = b.GetComponent<BossBullet>();
        if (bb != null) bb.SetDirection(dir);
    }

    public void TakeDamage(int amount = 1)
    {
        if (isDead) return;
        currentHealth -= amount;
        Debug.Log($"Boss: {currentHealth}/{maxHealth} HP");

        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);

        SpawnMinions();
        if (currentHealth <= 0) Die();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        Bullet b = other.GetComponent<Bullet>();
        if (b != null) TakeDamage(1);
    }

    void SpawnMinions()
    {
        if (minionPrefab == null) return;
        for (int i = 0; i < minionsPerHit; i++)
        {
            Vector3 offset = Random.insideUnitSphere * 2f;
            Instantiate(minionPrefab, transform.position + offset, Quaternion.identity);
        }
    }

    void Die()
    {
        isDead = true;
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}