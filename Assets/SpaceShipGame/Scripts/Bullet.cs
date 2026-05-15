using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Configuración")]
    public float speed = 80f;
    public float maxLifetime = 3f;
    public GameObject hitEffect;

    private Vector3 direction;
    private bool directionSet = false;

    void Start()
    {
        // Añadir collider automáticamente si no tiene ninguno
        if (GetComponent<Collider>() == null)
        {
            BoxCollider bc = gameObject.AddComponent<BoxCollider>();
            bc.isTrigger = true;
            bc.size = new Vector3(0.1f, 0.1f, 0.5f);
        }

        Destroy(gameObject, maxLifetime);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        directionSet = true;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
        transform.Rotate(90f, 0f, 0f, Space.Self);
    }

    void Update()
    {
        if (!directionSet) return;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        // Daño a enemigo normal
        EnemyController ec = other.GetComponentInParent<EnemyController>();
        if (ec != null)
        {
            ec.TakeDamage(1);
            SpawnHitEffect();
            Destroy(gameObject);
            return;
        }

        // Daño al boss
        BossController bc = other.GetComponentInParent<BossController>();
        if (bc != null)
        {
            bc.TakeDamage(1);
            SpawnHitEffect();
            Destroy(gameObject);
            return;
        }

        // Impacto con cualquier otra cosa (escenario, planetas...)
        SpawnHitEffect();
        Destroy(gameObject);
    }

    void SpawnHitEffect()
    {
        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);
    }
}