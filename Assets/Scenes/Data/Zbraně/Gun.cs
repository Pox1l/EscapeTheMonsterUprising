using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab støely
    [SerializeField] private Transform firePoint; // Bod, odkud støely vycházejí
    [SerializeField] private float fireRate = 0.5f; // Rychlost støelby (v sekundách)
    [SerializeField] private float bulletSpeed = 10f; // Rychlost støely
    [SerializeField] private float bulletLifeTime = 5f; // Doba života støely (v sekundách)
    [SerializeField] private int bulletDamage = 10; // Poškození støely

    private float nextFireTime; // Èas další støelby

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Bullet prefab or fire point is not assigned.");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.Rotate(0, 0, -90);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * bulletSpeed;
        }

        // Nastav damage støely
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = bulletDamage;
        }

        Destroy(bullet, bulletLifeTime);
        nextFireTime = Time.time + fireRate;
    }
}
