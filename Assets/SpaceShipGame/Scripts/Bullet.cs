using UnityEngine;

/// <summary>
/// Misión 02 — Bala del jugador
/// Añade este script al prefab Bullet.
/// El prefab debe tener Collider (Is Trigger = false) y Rigidbody (gravity = 0).
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Configuración")]
    public float speed          = 15f;
    public float maxLifetime    = 3f;    // auto-destrucción si no impacta 

    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
        Destroy(gameObject, maxLifetime);
    }

    void Update()
    {
        // Avanza hacia la derecha (dirección "adelante" en lateral 2.5D)
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // No reaccionar con el propio jugador
        if (collision.gameObject.CompareTag("Player")) return;


        Destroy(gameObject);
    }
}
