using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance; // Singleton instance

    public int maxHealth = 100; // Maximální HP
    private int currentHealth; // Aktuální HP

    private TextMeshProUGUI healthText; // TextMeshPro pro zobrazení HP
    private Slider healthSlider; // Slider pro vizuální zobrazení HP

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Uchová objekt mezi scénami
        }
        else
        {
            Destroy(gameObject); // Znièí duplicitní instance
        }
    }

    private void Start()
    {
        // Inicializujeme zdraví na základì toho, zda už bylo nastaveno pøi naètení scény
        if (currentHealth == 0) // Nastaví zdraví pouze pøi prvním spuštìní
        {
            currentHealth = maxHealth;
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // Pøidá logiku pro naètení nové scény
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scéna naètena: {scene.name}");

        // Najde konkrétní objekt podle názvu
        GameObject healthTextObject = GameObject.Find("HealthText");
        if (healthTextObject != null)
        {
            healthText = healthTextObject.GetComponent<TextMeshProUGUI>();
            Debug.Log("healthText byl nalezen a pøiøazen!");
        }
        else
        {
            Debug.LogWarning("HealthText objekt nebyl nalezen ve scénì!");
        }

        GameObject healthSliderObject = GameObject.Find("HealthSlider");
        if (healthSliderObject != null)
        {
            healthSlider = healthSliderObject.GetComponent<Slider>();
            Debug.Log("healthSlider byl nalezen a pøiøazen!");

            // Inicializuje hodnoty slideru
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("HealthSlider objekt nebyl nalezen ve scénì!");
        }

        UpdateHealthUI(); // Aktualizuje UI, pokud jsou objekty nalezeny
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
            Debug.LogWarning("healthText není pøiøazen!");
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("healthSlider není pøiøazen!");
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Odhlásí událost pøi znièení objektu
    }
}
