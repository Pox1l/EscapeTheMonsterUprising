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

    public int CurrentHealth => currentHealth; // Z�sk�n� aktu�ln�ho zdrav�
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

        filePath = Path.Combine(Application.persistentDataPath, "playerHealth.json"); // Ulo�en� cesty pro soubor
    }

    private void Start()
    {
        LoadHealth(); // Na��t�n� zdrav� p�i startu

        if (currentHealth == 0)
        {
            currentHealth = maxHealth;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    public void SetHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth); // Nastaven� zdrav� s validac�
        UpdateHealthUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Sc�na na�tena: {scene.name}");

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

        if (currentHealth != lastHealth) // Ukl�d�me pouze pokud do�lo ke zm�n�
        {
            SaveHealthAsync(); // Asynchronn� ulo�en� zdrav�
            lastHealth = currentHealth; // Ulo�en� aktu�ln�ho zdrav� pro srovn�n�
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

        if (currentHealth != lastHealth) // Ukl�d�me pouze pokud do�lo ke zm�n�
        {
            SaveHealthAsync(); // Asynchronn� ulo�en� zdrav�
            lastHealth = currentHealth; // Ulo�en� aktu�ln�ho zdrav� pro srovn�n�
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

    // Asynchronn� ulo�en� zdrav� do souboru JSON
    private async void SaveHealthAsync()
    {
        PlayerHealthData data = new PlayerHealthData();
        data.health = currentHealth;

        string json = JsonUtility.ToJson(data);

        // Asynchronn� z�pis do souboru
        await Task.Run(() => File.WriteAllText(filePath, json));
    }

    // Na�ten� zdrav� ze souboru JSON
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
            currentHealth = maxHealth; // Pokud soubor neexistuje, nastav�me z�kladn� hodnotu
        }
    }

    // Pomocn� t��da pro ulo�en� zdrav�
    [System.Serializable]
    public class PlayerHealthData
    {
        public int health;
    }

    // Zavol�me ulo�en� zdrav� p�i ukon�en� aplikace
    private void OnApplicationQuit()
    {
        SaveHealthAsync(); // Asynchronn� ulo�en� p�i ukon�en� aplikace
    }

    private int lastHealth = -1; // Pomocn� prom�nn� pro porovn�n� zdrav�
}
