using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Damage Settings")]
    public int damageToTarget = 10;
    public float damageRange = 1.5f;
    public float damageInterval = 1.0f;

    //[Header("XP")]
    //public GameObject xpPrefab;

    [Header("Particle")]
    [SerializeField] private ParticleSystem damageParticle;

    private float lastDamageTime;
    private EnemyController controller;

    void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<EnemyController>();

        if (controller == null)
        {
            Debug.LogError("EnemyController nebyl nalezen!");
        }
    }

    void Update()
    {
        if (controller == null) return;

        Transform target = controller.GetCurrentTarget();
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= damageRange && Time.time >= lastDamageTime + damageInterval)
        {
            if (target.CompareTag("Player"))
            {
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageToTarget);
                    lastDamageTime = Time.time;
                }
            }
            else if (target.CompareTag("NPC"))
            {
                NPCHealth npcHealth = target.GetComponent<NPCHealth>();
                if (npcHealth != null)
                {
                    npcHealth.TakeDamage(damageToTarget);
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
        //if (xpPrefab != null)
        //{
        //    Instantiate(xpPrefab, transform.position, Quaternion.identity);
        //}

        Destroy(gameObject);
    }

    private void SpawnDamageParticle()
    {
        if (damageParticle != null)
        {
            Instantiate(damageParticle, transform.position, Quaternion.identity);
        }
    }
}
