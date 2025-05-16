using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject); // Nebo nìjaká animace
    }
}
