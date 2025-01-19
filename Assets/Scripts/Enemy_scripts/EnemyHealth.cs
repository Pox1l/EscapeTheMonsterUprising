using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 50; // Maxim�ln� zdrav� nep��tele
    private int currentHealth; // Aktu�ln� zdrav� nep��tele

    [Header("Damage Settings")]
    public int damageToPlayer = 10; // Po�kozen� zp�soben� hr��i
    public float damageRange = 1.5f; // Maxim�ln� vzd�lenost k hr��i pro �tok
    public float damageInterval = 1.0f; // Interval mezi �toky (v sekund�ch)

    private Transform player; // Odkaz na hr��e
    private bool isPlayerInRange = false; // Kontroluje, zda je hr�� v dosahu
    private float lastDamageTime; // �as posledn�ho po�kozen�

    void Start()
    {
        currentHealth = maxHealth; // Inicializace zdrav�
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform; // Najdi hr��e podle tagu
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
                    playerHealth.TakeDamage(damageToPlayer); // Zp�sob po�kozen� hr��i
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
        Destroy(gameObject); // Odstran� nep��tele ze sc�ny
    }
}
