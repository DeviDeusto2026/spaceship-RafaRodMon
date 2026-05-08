using UnityEngine;

/// <summary>
/// Misión 02 + Bonus — Sistema de Disparo y Bomba
/// Añade este script a la nave del jugador.
/// Arrastra los prefabs Bullet y Bomb en el Inspector.
/// </summary>
public class ShootingController : MonoBehaviour
{
    [Header("Bala")]
    public GameObject bulletPrefab;
    public Transform  firePoint;        // Objeto vacío delante de la nave
    public float      fireRate = 0.2f;  // segundos entre disparos

    [Header("Bomba (Bonus)")]
    public GameObject bombPrefab;
    public int        maxBombs      = 3;
    public float      bombCooldown  = 1f;

    private float nextFireTime = 0f;
    private float nextBombTime = 0f;
    private int   currentBombs;

    void Start()
    {
        currentBombs = maxBombs;
    }

    void Update()
    {
        // Clic izquierdo → disparo
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }

        // Clic derecho → bomba (BONUS)
        if (Input.GetMouseButtonDown(1) && Time.time >= nextBombTime && currentBombs > 0)
        {
            nextBombTime = Time.time + bombCooldown;
            LaunchBomb();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    void LaunchBomb()
    {
        if (bombPrefab == null || firePoint == null) return;
        currentBombs--;
        Instantiate(bombPrefab, firePoint.position, firePoint.rotation);
        Debug.Log($"Bombas restantes: {currentBombs}");
    }

    // Llamado desde UI u otros scripts para consultar bombas
    public int GetBombCount() => currentBombs;
}
