using UnityEngine;

/// <summary>
/// Script de diagnóstico — añádelo a la nave del jugador.
/// Mira la Console mientras juegas y dime qué mensajes aparecen.
/// BÓRRALO cuando termines el diagnóstico.
/// </summary>
public class CollisionDebug : MonoBehaviour
{
    void Start()
    {
        // ── Reportar estado de la nave ────────────────────────────────────
        Debug.Log("=== DIAGNÓSTICO DE COLISIONES ===");
        Debug.Log($"Tag de la nave: '{gameObject.tag}'");
        Debug.Log($"Layer de la nave: {LayerMask.LayerToName(gameObject.layer)}");

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            Debug.Log($"Rigidbody: OK — isKinematic={rb.isKinematic}, useGravity={rb.useGravity}");
        else
            Debug.LogError("Rigidbody: NO TIENE — necesario para detectar colisiones");

        Collider[] cols = GetComponentsInChildren<Collider>();
        if (cols.Length == 0)
        {
            Debug.LogError("Colliders: NINGUNO — la nave no puede detectar nada sin collider");
        }
        else
        {
            foreach (Collider c in cols)
                Debug.Log($"Collider: {c.GetType().Name} en '{c.gameObject.name}' — isTrigger={c.isTrigger}");
        }

        PlayerHealth ph = GetComponent<PlayerHealth>();
        if (ph == null)
            Debug.LogError("PlayerHealth: NO ENCONTRADO en la nave");
        else
            Debug.Log("PlayerHealth: OK");
    }

    // Detecta cualquier trigger
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[TRIGGER] Nave tocada por: '{other.gameObject.name}' | tag='{other.tag}' | layer={LayerMask.LayerToName(other.gameObject.layer)}");
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log($"[TRIGGER STAY] Dentro de: '{other.gameObject.name}'");
    }

    // Detecta cualquier colisión física
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[COLLISION] Nave golpeada por: '{collision.gameObject.name}' | tag='{collision.gameObject.tag}' | layer={LayerMask.LayerToName(collision.gameObject.layer)}");
    }
}
