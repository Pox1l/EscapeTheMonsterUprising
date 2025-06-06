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

    // TakeDamage bere i smìr zásahu
    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        currentHealth -= damage;
        SpawnDamageParticle(hitDirection);

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

    private void SpawnDamageParticle(Vector2 hitDirection)
    {
        if (damageParticle != null)
        {
            ParticleSystem ps = Instantiate(damageParticle, transform.position, Quaternion.identity);

            // Otoèí particle opaènì než smìr zásahu (krvavý efekt vyletí z druhé strany)
            float angle = Mathf.Atan2(-hitDirection.y, -hitDirection.x) * Mathf.Rad2Deg;
            ps.transform.rotation = Quaternion.Euler(0, 0, angle);

            ps.Play();
        }
    }
}
