using UnityEngine;
using UnityEngine.UI;


public class BossController : MonoBehaviour
{
    public int maxHealth = 15;
    public Slider healthBar;


    public float chaseSpeed = 2f;      // velocidad hacia el jugador
    public float stopDistance = 8f;      // distancia a la que se detiene

    public float sinAmplitude = 2f;
    public float sinFrequency = 1f;

 
    public GameObject bossBulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;


    public GameObject minionPrefab;
    public int minionsPerHit = 1;

    private int currentHealth;
    private float timeAlive = 0f;
    private float nextFireTime = 0f;
    private Transform player;
    private bool isDead = false;

    private Vector3 sinAxis;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("BossController: no se encontró objeto con tag 'Player'");

        sinAxis = Vector3.up;

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
        if (healthBar != null)
            healthBar.value = currentHealth;
        Debug.Log($"Boss: {currentHealth}/{maxHealth} HP");
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
        if (VictoryManager.Instance != null)
            VictoryManager.Instance.OnBossKilled();

        Destroy(gameObject);
    }
}