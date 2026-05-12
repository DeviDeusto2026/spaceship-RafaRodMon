using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [Header("Cañones (orden: 0 = izquierdo, 1 = derecho)")]
    public Transform[] firePoints = new Transform[2];  // arrastra FirePointL y FirePointR

    [Header("Bala")]
    public GameObject bulletPrefab;
    public float fireRate = 0.15f;   // segundos entre disparos

    [Header("Bomba (Bonus)")]
    public GameObject bombPrefab;
    public int maxBombs = 3;
    public float bombCooldown = 1f;

    // ── Estado interno ────────────────────────────────────────────────────
    private int currentCannon = 0;     // 0 = izquierdo, 1 = derecho
    private float nextFireTime = 0f;
    private float nextBombTime = 0f;
    private int currentBombs;

    void Start()
    {
        currentBombs = maxBombs;
    }

    void Update()
    {
        // Clic izquierdo → disparo alterno
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }

        // Clic derecho → bomba
        if (Input.GetMouseButtonDown(1) && Time.time >= nextBombTime && currentBombs > 0)
        {
            nextBombTime = Time.time + bombCooldown;
            LaunchBomb();
        }
    }

    // ── Disparo alterno ───────────────────────────────────────────────────
    void Shoot()
    {
        if (bulletPrefab == null) return;

        // Seleccionar el cañón activo
        Transform fp = GetActiveFirePoint();
        if (fp == null) return;

        Instantiate(bulletPrefab, fp.position, fp.rotation);

        // Alternar al siguiente cañón: 0→1→0→1...
        currentCannon = (currentCannon + 1) % firePoints.Length;
    }

    Transform GetActiveFirePoint()
    {
        // Buscar el primer firePoint válido empezando por currentCannon
        for (int i = 0; i < firePoints.Length; i++)
        {
            int index = (currentCannon + i) % firePoints.Length;
            if (firePoints[index] != null)
                return firePoints[index];
        }
        return null;
    }

    // ── Bomba ─────────────────────────────────────────────────────────────
    void LaunchBomb()
    {
        if (bombPrefab == null) return;

        // La bomba sale del cañón activo en ese momento
        Transform fp = GetActiveFirePoint();
        if (fp == null) return;

        currentBombs--;
        Instantiate(bombPrefab, fp.position, fp.rotation);
        Debug.Log($"Bombas restantes: {currentBombs}");
    }

    public int GetBombCount() => currentBombs;
}