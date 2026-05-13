using UnityEngine;

/// <summary>
/// BONUS — Bomba de área
/// Añade este script al prefab Bomb.
/// Avanza hacia la derecha, explota tras N segundos y destruye TODOS los enemigos en pantalla.
/// </summary>
public class Bomb : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 6f;

    [Header("Explosión")]
    public float fuseTime       = 3f;    // segundos antes de explotar
    public GameObject explosionEffect;   // efecto de partículas opcional

    void Start()
    {
        Invoke(nameof(Explode), fuseTime);
    }

    void Update()
    {
        // Avanza hacia la derecha (igual que las balas)
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void Explode()
    {
        // Efecto visual
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Destruir TODOS los enemigos en escena
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            // Intentar reproducir efecto de muerte si tienen uno
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec != null)
            {
                // Llamar TakeDamage con un valor alto para matarlos
                ec.TakeDamage(999);
            }
            else
            {
                // Fallback: destruir directamente (sirve para el Boss también)
                BossController bc = enemy.GetComponent<BossController>();
                if (bc != null)
                    bc.TakeDamage(999);
                else
                    Destroy(enemy);
            }
        }

        Destroy(gameObject);
    }

    // Si la bomba choca con algo antes de explotar, también explota
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            CancelInvoke(nameof(Explode));
            Explode();
        }
    }
}
