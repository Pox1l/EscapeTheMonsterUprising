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

    [Header("Monster Type")]
    public bool isSpider = false; // Jestli je monstrum pavouk
    public bool isBig = false; // Jestli je monstrum velké
    public bool isFat = false; // Jestli je monstrum tlusté
    public bool isLittle = false; // Jestli je monstrum malé

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

                    // Aplikování efektù podle typu monstra
                    ApplyMonsterEffects(playerHealth);

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

    private void ApplyMonsterEffects(PlayerHealth playerHealth)
    {
        if (isSpider)
        {
            float poisonDuration = 5f; // Délka trvání otravy
            playerHealth.ApplyPoisonEffect(poisonDuration); // Efekt otravy pro pavouky
        }

        if (isBig)
        {
            float slowDuration = 3f; // Délka trvání zpomalení
            playerHealth.ApplySlowEffect(slowDuration); // Efekt zpomalení pro velké monstra
        }

        if (isFat)
        {
            float bleedDuration = 5f; // Délka trvání krvácení
            playerHealth.ApplyBleedEffect(bleedDuration); // Efekt krvácení pro tlustá monstra
        }

        if (isLittle)
        {
            // Mùžete pøidat vlastní efekt pro malá monstra, pokud chcete
        }
    }
}
