using UnityEngine;
using TMPro; // Pro TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximální HP
    private int currentHealth; // Aktuální HP

    public TextMeshProUGUI healthText; // TextMeshPro pro zobrazení HP

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ujistíme se, že HP je v rozmezí 0-maxHealth

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
        // Pøidej herní logiku pro smrt hráèe, napø. restart hry nebo konec hry
    }
}
