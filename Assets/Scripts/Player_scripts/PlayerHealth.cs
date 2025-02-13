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

        filePath = Path.Combine(Application.persistentDataPath, "playerHealth.json"); // Uložení cesty pro soubor
    }

    private void Start()
    {
        LoadHealth(); // Naèítání zdraví pøi startu

        if (currentHealth == 0)
        {
            currentHealth = maxHealth;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    public void SetHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth); // Nastavení zdraví s validací
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth != lastHealth) // Ukládáme pouze pokud došlo ke zmìnì
        {
            SaveHealthAsync(); // Asynchronní uložení zdraví
            lastHealth = currentHealth; // Uložení aktuálního zdraví pro srovnání
        }

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

        if (currentHealth != lastHealth) // Ukládáme pouze pokud došlo ke zmìnì
        {
            SaveHealthAsync(); // Asynchronní uložení zdraví
            lastHealth = currentHealth; // Uložení aktuálního zdraví pro srovnání
        }

        UpdateHealthUI();
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
    }

    // Asynchronní uložení zdraví do souboru JSON
    private async void SaveHealthAsync()
    {
        PlayerHealthData data = new PlayerHealthData();
        data.health = currentHealth;

        string json = JsonUtility.ToJson(data);

        // Asynchronní zápis do souboru
        await Task.Run(() => File.WriteAllText(filePath, json));
    }

    // Naètení zdraví ze souboru JSON
    private void LoadHealth()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerHealthData data = JsonUtility.FromJson<PlayerHealthData>(json);
            currentHealth = data.health;
        }
        else
        {
            currentHealth = maxHealth; // Pokud soubor neexistuje, nastavíme základní hodnotu
        }
    }

    // Pomocná tøída pro uložení zdraví
    [System.Serializable]
    public class PlayerHealthData
    {
        public int health;
    }

    // Zavoláme uložení zdraví pøi ukonèení aplikace
    private void OnApplicationQuit()
    {
        SaveHealthAsync(); // Asynchronní uložení pøi ukonèení aplikace
    }

    private int lastHealth = -1; // Pomocná promìnná pro porovnání zdraví
}
