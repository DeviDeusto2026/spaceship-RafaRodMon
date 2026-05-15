using UnityEngine;

/// <summary>
/// BONUS — Bomba de área
/// Añade este script al prefab Bomb.
/// Avanza hacia la derecha, explota tras N segundos y destruye TODOS los enemigos en pantalla.
/// </summary>
public class Bomb : MonoBehaviour
{
    private Vector3 direction;
    private bool directionSet = false;

    public float speed = 6f;

    public float fuseTime       = 3f;    

    void Start()
    {
        Invoke(nameof(Explode), fuseTime);
    }

    void Update()
    {
        if (!directionSet) return;
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        directionSet = true;
    }

    void Explode()
    {

        // Destruir TODOS los enemigos en escena
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            // Intentar reproducir efecto de muerte si tienen uno
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec != null)
            {
                // Llamar TakeDamage con un valor alto para matarlos
                ec.TakeDamage(4);
            }
            else
            {
                // Fallback: destruir directamente (sirve para el Boss también)
                BossController bc = enemy.GetComponent<BossController>();
                if (bc != null)
                    bc.TakeDamage(4);
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
