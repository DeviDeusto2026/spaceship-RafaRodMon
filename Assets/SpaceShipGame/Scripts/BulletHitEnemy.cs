using UnityEngine;

public class BulletHitEnemy : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        // Daño a enemigo normal
        EnemyController ec = collision.gameObject.GetComponent<EnemyController>();
        if (ec != null) ec.TakeDamage(1);


        Destroy(gameObject);
    }
}
