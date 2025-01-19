using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab st�ely
    [SerializeField] private Transform firePoint; // Bod, odkud st�ely vych�zej�
    [SerializeField] private float fireRate = 0.5f; // Rychlost st�elby (v sekund�ch)
    [SerializeField] private float bulletSpeed = 10f; // Rychlost st�ely
    [SerializeField] private float bulletLifeTime = 5f; // Doba �ivota st�ely (v sekund�ch)

    private float nextFireTime; // �as dal�� st�elby

    void Update()
    {
        // St�elba p�i dr�en� lev�ho tla��tka my�i
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

        // Vytvo� st�elu
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Oto� kulku o 90 stup�� na ose Z
        bullet.transform.Rotate(0, 0, -90);

        // P�idej pohyb st�ele
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Nastav rychlost st�ely podle sm�ru firePoint
            rb.velocity = firePoint.right * bulletSpeed;
        }

        // Zni� st�elu po ur�it� dob�
        Destroy(bullet, bulletLifeTime);

        // Nastav �as dal�� st�elby
        nextFireTime = Time.time + fireRate;
    }
}