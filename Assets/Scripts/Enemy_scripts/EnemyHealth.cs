using UnityEngine;

public class EnemyDamageAndHealth : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 50; // Maxim�ln� zdrav� nep��tele
    private int currentHealth; // Aktu�ln� zdrav� nep��tele

    [Header("Damage Settings")]
    public int damageToPlayer = 10; // Po�kozen� zp�soben� hr��i
    public float damageCooldown = 1f; // Interval mezi �toky
    public float damageRange = 1.5f; // Maxim�ln� vzd�lenost k hr��i pro �tok

    private Transform player; // Odkaz na hr��e
    private float lastDamageTime; // �as posledn�ho �toku

    void Start()
    {
        currentHealth = maxHealth; // Inicializace zdrav�
        player = GameObject.FindGameObjectWithTag("Player").transform; // Najdi hr��e podle tagu
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Pokud je hr�� v dosahu, prove� �tok
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
        Destroy(gameObject); // Odstran� nep��tele ze sc�ny
    }
}
