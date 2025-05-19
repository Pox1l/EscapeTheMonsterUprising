using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    /* ---------- základní HP ---------- */
    [Header("HP")]
    public int maxHealth = 100;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

    /* ---------- pasivní REGEN ---------- */
    [Header("Passive regen")]
    [HideInInspector] public float regenPerSec = 0f;     // určuje UpgradeManager
    [SerializeField] private float regenTick = 0.25f;     //  nastavitelné v Inspectoru

    private float regenTimer = 0f;


    /* ---------- UI ---------- */
    private TextMeshProUGUI healthText;
    private Slider healthSlider;

    /* ---------- ukládání ---------- */
    private string filePath;
    private int lastHealth = -1;

    /* ---------- singleton ---------- */
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
            return;
        }

        filePath = Path.Combine(Application.persistentDataPath, "playerHealth.json");
    }

    /* ---------- init ---------- */
    private void Start()
    {
        LoadHealth();

        if (currentHealth <= 0) currentHealth = maxHealth;

        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    /* ---------- pasivní update ---------- */
    private void Update()
    {
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

    /* ---------- veřejné API ---------- */
    public void SetHealth(int hp)
    {
        currentHealth = Mathf.Clamp(hp, 0, maxHealth);
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        ScreenEffectController.Instance?.PlayHealEffect();
        SaveIfChanged();
        UpdateHealthUI();
    }

    public void TakeDamage(int dmg)
    {
        if (dmg <= 0) return;

        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);
        ScreenEffectController.Instance?.PlayDamageEffect();
        SaveIfChanged();
        UpdateHealthUI();

        if (currentHealth <= 0) Die();
    }

    /* ---------- UI & scene ---------- */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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

    /* ---------- smrt ---------- */
    private void Die()
    {
        Debug.Log("Player has died!");
        GameOverManager gm = FindObjectOfType<GameOverManager>();
        if (gm) gm.ShowGameOverUI();
        else Debug.LogError("GameOverManager nebyl nalezen!");
    }

    /* ---------- ulož / načti ---------- */
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

    /* ---------- datová třída ---------- */
    [System.Serializable] private class PlayerHealthData { public int health; }
}
