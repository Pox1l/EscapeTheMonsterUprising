using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab støely
    [SerializeField] private Transform firePoint; // Bod, odkud støely vycházejí
    [SerializeField] private float fireRate = 0.5f; // Rychlost støelby (v sekundách)
    [SerializeField] private float bulletSpeed = 10f; // Rychlost støely
    [SerializeField] private float bulletLifeTime = 5f; // Doba života støely (v sekundách)
    [SerializeField] private int bulletDamage = 10; // Poškození støely

    [SerializeField] private bool isShotgun = false; // Pokud je zbraò brokovnice
    [SerializeField] private int pelletCount = 5; // Poèet projektilù pro brokovnici
    [SerializeField] private float spreadAngle = 15f; // Maximální rozptyl v úhlech


    private float nextFireTime; // Èas další støelby

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            AudioManager.instance.PlayGunShot();
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

        if (isShotgun)
        {
            for (int i = 0; i < pelletCount; i++)
            {
                float angle = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
                Quaternion rotation = Quaternion.Euler(0, 0, firePoint.eulerAngles.z + angle);

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
                bullet.transform.Rotate(0, 0, -90); // Pokud tvùj sprite potøebuje otoèku

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = rotation * Vector2.right * bulletSpeed;
                }

                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.damage = bulletDamage;
                }

                Destroy(bullet, bulletLifeTime);
            }
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.transform.Rotate(0, 0, -90);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = firePoint.right * bulletSpeed;
            }

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = bulletDamage;
            }

            Destroy(bullet, bulletLifeTime);
        }

        nextFireTime = Time.time + fireRate;
    }

}
