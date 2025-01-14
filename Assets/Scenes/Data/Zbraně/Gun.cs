using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab st�ely
    [SerializeField] private Transform firePoint; // Bod, odkud st�ely vych�zej�
    [SerializeField] private float fireRate = 0.5f; // Rychlost st�elby (v sekund�ch)
    [SerializeField] private float bulletSpeed = 10f; // Rychlost st�ely (bude nyn� pou�ita jako s�la)
    [SerializeField] private float bulletLifeTime = 5f; // Doba �ivota st�ely (v sekund�ch)
    [SerializeField] private string targetTag = "Monster"; // Tag, na kter� kulka reaguje

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
        // Vytvo� st�elu
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Vypo��tej sm�r s�ly
            Vector2 forceDirection = firePoint.right * bulletSpeed;

            // Pou�ij s�lu pro pohyb st�ely (impuls)
            rb.AddForce(forceDirection, ForceMode2D.Impulse); // Impulse pro okam�it� pohyb

            // Nastav�me, �e st�ela nebude ovlivn�n� gravitac� (pokud nen� pot�eba)
            rb.gravityScale = 0;

            // Rotace st�ely podle sm�ru, ve kter�m byla vyst�elena
            float angle = Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Po ur�it� dob� zni��me kulku
            Destroy(bullet, bulletLifeTime);
        }

        nextFireTime = Time.time + fireRate; // Nastav �as dal�� st�elby
        
    }

    // Funkce pro detekci kolize s objektem
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Zkontrolujeme, jestli kulka narazila na objekt s po�adovan�m tagem
        if (collision.gameObject.CompareTag(targetTag))
        {
            // M��ete zde p�idat logiku pro zpracov�n� kolize, pokud je pot�eba
            Debug.Log("Bullet hit: " + collision.gameObject.name);

            // Zni��me kulku p�i kolizi s objektem s po�adovan�m tagem
            Destroy(gameObject);
        }
    }
}
