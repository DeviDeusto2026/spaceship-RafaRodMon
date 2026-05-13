using UnityEngine;

/// <summary>
/// Helper: añade este script al prefab Bullet para que, al colisionar con un enemigo,
/// llame a TakeDamage en lugar de sólo destruirse.
/// Sustituye (o complementa) la lógica de OnCollisionEnter en Bullet.cs.
/// </summary>
public class BulletHitEnemy : MonoBehaviour
{
    public GameObject hitEffect;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        // Daño a enemigo normal
        EnemyController ec = collision.gameObject.GetComponent<EnemyController>();
        if (ec != null) ec.TakeDamage(1);

        // Daño al boss
        

        // Efecto de impacto
        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
