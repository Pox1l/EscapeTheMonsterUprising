using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    public int maxHealth = 100;
    private int currentHealth;

    private TextMeshProUGUI healthText;
    private Slider healthSlider;

    public int CurrentHealth => currentHealth; // Získání aktuálního zdraví
    private string filePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        filePath = Path.Combine(Application.persistentDataPath, "playerHealth.json");
    }

    private void Start()
    {
        LoadHealth();

        if (currentHealth == 0)
        {
            currentHealth = maxHealth;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    public void SetHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scéna naètena: {scene.name}");

        GameObject healthTextObject = GameObject.Find("HealthText");
        if (healthTextObject != null)
        {
            healthText = healthTextObject.GetComponent<TextMeshProUGUI>();
        }

        GameObject healthSliderObject = GameObject.Find("HealthSlider");
        if (healthSliderObject != null)
        {
            healthSlider = healthSliderObject.GetComponent<Slider>();
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        UpdateHealthUI();
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth != lastHealth) // Ukládáme pouze pokud došlo ke zmìnì
        {
            SaveHealthAsync(); // Asynchronní uložení zdraví
            lastHealth = currentHealth; // Uložení aktuálního zdraví pro srovnání
        }

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth != lastHealth)
        {
            SaveHealthAsync();
            lastHealth = currentHealth;
        }

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

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");

        // Najde GameOverManager ve scénì a aktivuje GameOver UI
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOverUI();
        }
        else
        {
            Debug.LogError("GameOverManager nebyl nalezen ve scénì!");
        }
    }


    private async void SaveHealthAsync()
    {
        PlayerHealthData data = new PlayerHealthData();
        data.health = currentHealth;

        string json = JsonUtility.ToJson(data);

        await Task.Run(() => File.WriteAllText(filePath, json));
    }

    private void LoadHealth()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerHealthData data = JsonUtility.FromJson<PlayerHealthData>(json);
            currentHealth = data.health;
        }

        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            SaveHealthAsync();
        }
    }

    [System.Serializable]
    public class PlayerHealthData
    {
        public int health;
    }

    private void OnApplicationQuit()
    {
        SaveHealthAsync();
    }

    private int lastHealth = -1;
}
