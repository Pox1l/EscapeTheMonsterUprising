using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    public int maxHealth = 100;
    private int currentHealth;

    private TextMeshProUGUI healthText;
    private Slider healthSlider;

    public Image poisonIcon;
    public Image bleedIcon;
    public Image slowIcon;
    public TextMeshProUGUI poisonTimerText;
    public TextMeshProUGUI bleedTimerText;
    public TextMeshProUGUI slowTimerText;

    private bool isPoisoned = false;
    private bool isBleeding = false;
    private bool isSlowed = false;

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
    }

    private void Start()
    {
        if (currentHealth == 0)
        {
            currentHealth = maxHealth;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scéna naètena: {scene.name}");

        if (scene.name == "Game_Inside") // Pokud jsme v lobby
        {
            ResetEffects(); // Reset efektù pøi návratu do lobby
        }

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
        UpdateEffectUI(); // Aktualizuje viditelnost efektù podle scény
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

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
    }

    public void ApplyPoisonEffect(float duration)
    {
        if (!isPoisoned)
        {
            isPoisoned = true;
            poisonIcon.gameObject.SetActive(true);
            StartCoroutine(PoisonDamage());
            StartCoroutine(PoisonTimer(duration));
        }
    }

    public void ApplyBleedEffect(float duration)
    {
        if (!isBleeding)
        {
            isBleeding = true;
            bleedIcon.gameObject.SetActive(true);
            StartCoroutine(BleedDamage());
            StartCoroutine(BleedTimer(duration));
        }
    }

    public void ApplySlowEffect(float duration)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            slowIcon.gameObject.SetActive(true);
            StartCoroutine(SlowEffect());
            StartCoroutine(SlowTimer(duration));
        }
    }

    private IEnumerator PoisonDamage()
    {
        while (isPoisoned)
        {
            currentHealth -= 2;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator BleedDamage()
    {
        while (isBleeding)
        {
            currentHealth -= 5;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator PoisonTimer(float duration)
    {
        while (duration > 0)
        {
            poisonTimerText.text = $"Poison: {Mathf.Ceil(duration)}s";
            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }
        poisonIcon.gameObject.SetActive(false);
        isPoisoned = false;
    }

    private IEnumerator BleedTimer(float duration)
    {
        while (duration > 0)
        {
            bleedTimerText.text = $"Bleed: {Mathf.Ceil(duration)}s";
            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }
        bleedIcon.gameObject.SetActive(false);
        isBleeding = false;
    }

    private IEnumerator SlowTimer(float duration)
    {
        while (duration > 0)
        {
            slowTimerText.text = $"Slow: {Mathf.Ceil(duration)}s";
            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }
        slowIcon.gameObject.SetActive(false);
        isSlowed = false;
    }

    private IEnumerator SlowEffect()
    {
        while (isSlowed)
        {
            yield return null;
        }
    }

    private void ResetEffects()
    {
        isPoisoned = false;
        isBleeding = false;
        isSlowed = false;

        poisonIcon?.gameObject.SetActive(false);
        bleedIcon?.gameObject.SetActive(false);
        slowIcon?.gameObject.SetActive(false);

        poisonTimerText?.SetText("");
        bleedTimerText?.SetText("");
        slowTimerText?.SetText("");

        Debug.Log("Všechny efekty resetovány.");
    }

    private void UpdateEffectUI()
    {
        bool showEffects = SceneManager.GetActiveScene().name == "Game_Outside";

        poisonIcon?.gameObject.SetActive(showEffects && isPoisoned);
        bleedIcon?.gameObject.SetActive(showEffects && isBleeding);
        slowIcon?.gameObject.SetActive(showEffects && isSlowed);

        poisonTimerText?.gameObject.SetActive(showEffects);
        bleedTimerText?.gameObject.SetActive(showEffects);
        slowTimerText?.gameObject.SetActive(showEffects);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
