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

    [Header("XP Settings")]
    public GameObject xpPrefab; // Prefab XP, který se spawnuje pøi smrti

    [Header("Particle")]
    [SerializeField] private ParticleSystem damageParticle;

    private ParticleSystem damageParticleInstance;

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
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        SpawnDamageParticle();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (xpPrefab != null)
        {
            Instantiate(xpPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("XP prefab is not assigned to enemy!");
        }

        Destroy(gameObject); // Odstraní nepøítele ze scény
    }

    private void SpawnDamageParticle()
    {
        if (damageParticle != null && player != null)
        {
            // Vektor smìrem od hráèe k nepøíteli (v 2D)
            Vector2 direction = (Vector2)(transform.position - player.position);

            // Úhel otoèení ve stupních (pro 2D)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Otoèení efektu tak, aby smìøoval od hráèe
            Quaternion particleRotation = Quaternion.Euler(0, 0, angle);

            // Spawn èástic s otoèením
            damageParticleInstance = Instantiate(damageParticle, transform.position, particleRotation);
        }
    }

}
