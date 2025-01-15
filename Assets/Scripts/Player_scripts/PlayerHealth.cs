using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance; // Singleton instance

    public int maxHealth = 100; // Maxim�ln� HP
    private int currentHealth; // Aktu�ln� HP

    private TextMeshProUGUI healthText; // TextMeshPro pro zobrazen� HP

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Uchov� objekt mezi sc�nami
        }
        else
        {
            Destroy(gameObject); // Zni�� duplicitn� instance
        }
    }

    private void Start()
    {
        if (currentHealth == 0) // Nastav� zdrav� pouze p�i prvn�m spu�t�n�
        {
            currentHealth = maxHealth;
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // P�id� logiku pro na�ten� nov� sc�ny
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Sc�na na�tena: {scene.name}");

        // Najde konkr�tn� objekt podle n�zvu
        GameObject healthTextObject = GameObject.Find("HealthText");
        if (healthTextObject != null)
        {
            healthText = healthTextObject.GetComponent<TextMeshProUGUI>();
            Debug.Log("healthText byl nalezen a p�i�azen!");
        }
        else
        {
            Debug.LogWarning("HealthText objekt nebyl nalezen ve sc�n�!");
        }

        UpdateHealthUI(); // Aktualizuje UI, pokud je nalezen
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth;
        }
        else
        {
            Debug.LogWarning("healthText nen� p�i�azen!");
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Odhl�s� ud�lost p�i zni�en� objektu
    }
}
