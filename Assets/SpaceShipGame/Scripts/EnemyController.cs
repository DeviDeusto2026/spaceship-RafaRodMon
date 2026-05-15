using UnityEngine;
using UnityEngine.UI;


public class EnemyController : MonoBehaviour
{
    public float speed = 4f;

    public int maxHealth = 3;

    public GameObject deathEffect;
    public Slider healthBar;  

    private Transform player;
    private int currentHealth;

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
            Debug.LogWarning("EnemyController: no se encontró objeto con tag 'Player'");


        // Asegurarse de tener collider para recibir balas
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            sc.radius = 1f;
            Debug.Log($"{gameObject.name}: Collider añadido automáticamente.");
        }
    }

    void Update()
    {
        if (player == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    public void TakeDamage(int amount = 1)
    {
        currentHealth -= amount;
        if (healthBar != null)
            healthBar.value = currentHealth;
        Debug.Log($"{gameObject.name} recibe {amount} de daño. Vida restante: {currentHealth}");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        // Notificar victoria
        if (VictoryManager.Instance != null)
            VictoryManager.Instance.OnEnemyKilled(); 
        Destroy(gameObject);
    }

    // Detectar impacto con bala (trigger)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        Bullet b = other.GetComponent<Bullet>();
        if (b != null) TakeDamage(1);
    }

    // Detectar contacto con el jugador (colisión física)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            if (ph != null) ph.Die();
        }
    }
}