using UnityEngine;

public class EnemyDamageAndHealth : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 50; // Maximální zdraví nepøítele
    private int currentHealth; // Aktuální zdraví nepøítele

    [Header("Damage Settings")]
    public int damageToPlayer = 10; // Poškození zpùsobené hráèi
    public float damageCooldown = 1f; // Interval mezi útoky
    public float damageRange = 1.5f; // Maximální vzdálenost k hráèi pro útok

    private Transform player; // Odkaz na hráèe
    private float lastDamageTime; // Èas posledního útoku

    void Start()
    {
        currentHealth = maxHealth; // Inicializace zdraví
        player = GameObject.FindGameObjectWithTag("Player").transform; // Najdi hráèe podle tagu
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Pokud je hráè v dosahu, proveï útok
            if (distanceToPlayer <= damageRange)
            {
                TryDamagePlayer();
            }
        }
    }

    private void TryDamagePlayer()
    {
        if (Time.time >= lastDamageTime + damageCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
                Debug.Log("Enemy damaged player for: " + damageToPlayer);
            }
            lastDamageTime = Time.time;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took damage: " + damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy has died!");
        Destroy(gameObject); // Odstraní nepøítele ze scény
    }
}
