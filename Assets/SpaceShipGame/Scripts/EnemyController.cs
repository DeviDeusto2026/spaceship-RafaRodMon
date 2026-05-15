using UnityEngine;

/// <summary>
/// Enemigo normal — persigue al jugador, recibe daño por trigger.
/// Requiere:
///   - Tag: "Enemy"
///   - Un Collider en el prefab (cualquier tipo, Is Trigger = true)
///   - El jugador con tag "Player"
/// </summary>
public class EnemyController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 4f;

    [Header("Vida")]
    public int maxHealth = 3;

    [Header("Efectos")]
    public GameObject deathEffect;
    public EnemyHealthUI healthBar;

    private Transform player;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("EnemyController: no se encontró objeto con tag 'Player'");

        // --- DEBUG: si el enemigo no se ve, revisar estos logs ---
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend == null)
            Debug.LogWarning($"{gameObject.name}: no tiene Renderer. No será visible.");
        else
            Debug.Log($"{gameObject.name}: Renderer OK — material: {rend.material.name}");

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
            healthBar.UpdateHealth(currentHealth, maxHealth);
        Debug.Log($"{gameObject.name} recibe {amount} de daño. Vida restante: {currentHealth}");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);
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