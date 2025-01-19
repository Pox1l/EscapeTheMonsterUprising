using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 50; // Maximální zdraví nepøítele
    private int currentHealth; // Aktuální zdraví nepøítele

    [Header("Damage Settings")]
    public int damageToPlayer = 10; // Poškození zpùsobené hráèi
    public float damageRange = 1.5f; // Maximální vzdálenost k hráèi pro útok
    public float damageInterval = 1.0f; // Interval mezi útoky (v sekundách)

    private Transform player; // Odkaz na hráèe
    private bool isPlayerInRange = false; // Kontroluje, zda je hráè v dosahu
    private float lastDamageTime; // Èas posledního poškození

    void Start()
    {
        currentHealth = maxHealth; // Inicializace zdraví
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform; // Najdi hráèe podle tagu
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            isPlayerInRange = distanceToPlayer <= damageRange;

            if (isPlayerInRange && Time.time >= lastDamageTime + damageInterval)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageToPlayer); // Zpùsob poškození hráèi
                    Debug.Log("Enemy damaged player over time!");
                    lastDamageTime = Time.time;
                }
            }
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
