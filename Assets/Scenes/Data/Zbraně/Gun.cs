using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab støely
    [SerializeField] private Transform firePoint; // Bod, odkud støely vycházejí
    [SerializeField] private float fireRate = 0.5f; // Rychlost støelby (v sekundách)
    [SerializeField] private float bulletSpeed = 10f; // Rychlost støely
    [SerializeField] private float bulletLifeTime = 5f; // Doba života støely (v sekundách)

    private float nextFireTime; // Èas další støelby

    void Update()
    {
        // Støelba pøi držení levého tlaèítka myši
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

        // Vytvoø støelu
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Otoè kulku o 90 stupòù na ose Z
        bullet.transform.Rotate(0, 0, -90);

        // Pøidej pohyb støele
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Nastav rychlost støely podle smìru firePoint
            rb.velocity = firePoint.right * bulletSpeed;
        }

        // Zniè støelu po urèité dobì
        Destroy(bullet, bulletLifeTime);

        // Nastav èas další støelby
        nextFireTime = Time.time + fireRate;
    }
}