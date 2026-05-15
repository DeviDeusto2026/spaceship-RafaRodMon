using UnityEngine;

public class ShootingController : MonoBehaviour
{

    public Transform[] firePoints = new Transform[2];

    public GameObject bulletPrefab;
    public float fireRate = 0.15f;

    public GameObject bombPrefab;
    public int maxBombs = 3;
    public float bombCooldown = 1f;

    private int currentCannon = 0;
    private float nextFireTime = 0f;
    private float nextBombTime = 0f;
    private int currentBombs;

    void Start()
    {
        currentBombs = maxBombs;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }

        if (Input.GetMouseButtonDown(1) && Time.time >= nextBombTime && currentBombs > 0)
        {
            nextBombTime = Time.time + bombCooldown;
            LaunchBomb();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;
        Transform fp = GetActiveFirePoint();
        if (fp == null) return;

        GameObject b = Instantiate(bulletPrefab, fp.position, Quaternion.identity);
        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet != null)
            bullet.SetDirection(transform.forward);

        // Ignorar colisión física entre la bala y TODOS los colliders de la nave
        Collider bulletCol = b.GetComponent<Collider>();
        if (bulletCol != null)
        {
            foreach (Collider shipCol in GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(bulletCol, shipCol, true);
        }

        currentCannon = (currentCannon + 1) % firePoints.Length;
    }

    Transform GetActiveFirePoint()
    {
        for (int i = 0; i < firePoints.Length; i++)
        {
            int index = (currentCannon + i) % firePoints.Length;
            if (firePoints[index] != null)
                return firePoints[index];
        }
        return null;
    }

    void LaunchBomb()
    {
        if (bombPrefab == null) return;
        Transform fp = GetActiveFirePoint();
        if (fp == null) return;
        currentBombs--;

        GameObject b = Instantiate(bombPrefab, fp.position, Quaternion.identity);
        Bomb bomb = b.GetComponent<Bomb>();
        if (bomb != null)
            bomb.SetDirection(transform.forward);
    }

    public int GetBombCount() => currentBombs;
}