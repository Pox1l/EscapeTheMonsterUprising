using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab støely
    [SerializeField] private Transform firePoint; // Bod, odkud støely vycházejí
    [SerializeField] private float fireRate = 0.5f; // Rychlost støelby (v sekundách)
    [SerializeField] private float bulletSpeed = 10f; // Rychlost støely (bude nyní použita jako síla)
    [SerializeField] private float bulletLifeTime = 5f; // Doba života støely (v sekundách)
    [SerializeField] private string targetTag = "Monster"; // Tag, na který kulka reaguje

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
        // Vytvoø støelu
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Vypoèítej smìr síly
            Vector2 forceDirection = firePoint.right * bulletSpeed;

            // Použij sílu pro pohyb støely (impuls)
            rb.AddForce(forceDirection, ForceMode2D.Impulse); // Impulse pro okamžitý pohyb

            // Nastavíme, že støela nebude ovlivnìná gravitací (pokud není potøeba)
            rb.gravityScale = 0;

            // Rotace støely podle smìru, ve kterém byla vystøelena
            float angle = Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Po urèité dobì znièíme kulku
            Destroy(bullet, bulletLifeTime);
        }

        nextFireTime = Time.time + fireRate; // Nastav èas další støelby
        
    }

    // Funkce pro detekci kolize s objektem
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Zkontrolujeme, jestli kulka narazila na objekt s požadovaným tagem
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Mùžete zde pøidat logiku pro zpracování kolize, pokud je potøeba
            Debug.Log("Bullet hit: " + collision.gameObject.name);

            // Znièíme kulku pøi kolizi s objektem s požadovaným tagem
            Destroy(gameObject);
        }
    }
}
