using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Header("HP")]
    public int maxHealth = 100;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

    [Header("Passive regen")]
    [HideInInspector] public float regenPerSec = 0f;
    [SerializeField] private float regenTick = 0.25f;
    private float regenTimer = 0f;

    private TextMeshProUGUI healthText;
    private Slider healthSlider;

    private string filePath;
    private int lastHealth = -1;
    private bool isDead = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        filePath = Path.Combine(Application.persistentDataPath, "playerHealth.json");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        LoadHealth();
        if (currentHealth <= 0) currentHealth = maxHealth;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void Update()
    {
        if (isDead) return;

        if (regenPerSec > 0f && currentHealth < maxHealth)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= regenTick)
            {
                float hpToAdd = regenPerSec * regenTimer;
                Heal(Mathf.RoundToInt(hpToAdd));
                regenTimer = 0f;
            }
        }
    }

    public void SetHealth(int hp)
    {
        currentHealth = Mathf.Clamp(hp, 0, maxHealth);
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        ScreenEffectController.Instance?.PlayHealEffect();
        AudioManager.instance.HealClip();
        SaveIfChanged();
        UpdateHealthUI();
    }

    public void TakeDamage(int dmg)
    {
        if (dmg <= 0 || isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);
        ScreenEffectController.Instance?.PlayDamageEffect();
        AudioManager.instance.DamageClip();
        SaveIfChanged();
        UpdateHealthUI();

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        isDead = true;

        GameOverManager gm = FindObjectOfType<GameOverManager>();
        AudioManager.instance.DeadClip();
        if (gm) gm.ShowGameOverUI();
        else Debug.LogError("GameOverManager nebyl nalezen!");
    }

    public void Respawn()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthUI();
        SaveIfChanged();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(WaitAndAssignUI());
    }

    private System.Collections.IEnumerator WaitAndAssignUI()
    {
        yield return null;
        yield return null;

        healthText = GameObject.Find("HealthText")?.GetComponent<TextMeshProUGUI>();
        healthSlider = GameObject.Find("HealthSlider")?.GetComponent<Slider>();

        if (healthSlider)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText) healthText.text = $"HP: {currentHealth}";
        if (healthSlider) healthSlider.value = currentHealth;
    }

    private async void SaveHealthAsync()
    {
        string json = JsonUtility.ToJson(new PlayerHealthData { health = currentHealth });
        await Task.Run(() => File.WriteAllText(filePath, json));
    }

    private void SaveIfChanged()
    {
        if (currentHealth == lastHealth) return;
        SaveHealthAsync();
        lastHealth = currentHealth;
    }

    private void LoadHealth()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            currentHealth = JsonUtility.FromJson<PlayerHealthData>(json).health;
        }
        else
        {
            currentHealth = maxHealth;
            SaveHealthAsync();
        }
        lastHealth = currentHealth;
    }

    private void OnApplicationQuit() => SaveHealthAsync();

    [System.Serializable]
    private class PlayerHealthData
    {
        public int health;
    }
}
