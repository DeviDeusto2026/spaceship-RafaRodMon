using UnityEngine;

/// <summary>
/// Bala del Boss — funciona igual que la bala del jugador.
/// Se mueve en la dirección asignada por SetDirection().
/// Tag: "EnemyBullet"
/// </summary>
public class BossBullet : MonoBehaviour
{
    public float speed = 20f;
    public float maxLifetime = 5f;

    private Vector3 direction;
    private bool directionSet = false;

    void Start()
    {
        // Añadir collider si no tiene
        if (GetComponent<Collider>() == null)
        {
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            sc.radius = 0.2f;
        }

        // Ignorar colisión con el propio Boss y sus hijos
        Collider myCol = GetComponent<Collider>();
        if (myCol != null)
        {
            // Buscar el boss por tag para ignorar sus colliders
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in enemies)
                foreach (Collider c in e.GetComponentsInChildren<Collider>())
                    Physics.IgnoreCollision(myCol, c, true);
        }

        Destroy(gameObject, maxLifetime);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        directionSet = true;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void Update()
    {
        if (!directionSet) return;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // Solo daña al jugador
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponentInParent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(1);  // respeta iframes y sistema de vidas
            Destroy(gameObject);
        }
    }
}