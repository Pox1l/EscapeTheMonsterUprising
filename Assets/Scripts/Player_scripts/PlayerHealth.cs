using UnityEngine;
using TMPro; // Pro TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maxim�ln� HP
    private int currentHealth; // Aktu�ln� HP

    public TextMeshProUGUI healthText; // TextMeshPro pro zobrazen� HP

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ujist�me se, �e HP je v rozmez� 0-maxHealth

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        Destroy(gameObject);
        // P�idej hern� logiku pro smrt hr��e, nap�. restart hry nebo konec hry
    }
}
