using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance; // Singleton instance

    public int maxHealth = 100; // Maximální HP
    private int currentHealth; // Aktuální HP

    private TextMeshProUGUI healthText; // TextMeshPro pro zobrazení HP
    private Slider healthSlider; // Slider pro vizuální zobrazení HP

    // UI pro zobrazení efektù
    public RawImage poisonIcon; // Ikona pro otravu
    public RawImage bleedIcon; // Ikona pro krvácení
    public RawImage slowIcon; // Ikona pro zpomalení
    public TextMeshProUGUI poisonTimerText; // Text pro odpoèítávání otravy
    public TextMeshProUGUI bleedTimerText; // Text pro odpoèítávání krvácení
    public TextMeshProUGUI slowTimerText; // Text pro odpoèítávání zpomalení

    // Efekty
    private bool isPoisoned = false;
    private bool isBleeding = false;
    private bool isSlowed = false;
    private float poisonTimer = 0f;
    private float bleedTimer = 0f;
    private float slowTimer = 0f;

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
        if (currentHealth == 0) // Nastaví zdraví pouze pøi prvním spuštìní
        {
            currentHealth = maxHealth;
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // Pøidá logiku pro naètení nové scény
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scéna naètena: {scene.name}");

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

            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("HealthSlider objekt nebyl nalezen ve scénì!");
        }

        UpdateHealthUI(); // Aktualizuje UI
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

    public void ApplyPoisonEffect(float duration)
    {
        if (poisonIcon == null || poisonTimerText == null) return; // Kontrola, zda existují UI elementy

        if (!isPoisoned)
        {
            isPoisoned = true;
            poisonTimer = duration;
            poisonIcon.gameObject.SetActive(true); // Zobrazí ikonu pro otravu
            StartCoroutine(PoisonDamage());
            StartCoroutine(PoisonTimer(duration));
        }
    }

    public void ApplyBleedEffect(float duration)
    {
        if (bleedIcon == null || bleedTimerText == null) return; // Kontrola, zda existují UI elementy

        if (!isBleeding)
        {
            isBleeding = true;
            bleedTimer = duration;
            bleedIcon.gameObject.SetActive(true); // Zobrazí ikonu pro krvácení
            StartCoroutine(BleedDamage());
            StartCoroutine(BleedTimer(duration));
        }
    }

    public void ApplySlowEffect(float duration)
    {
        if (slowIcon == null || slowTimerText == null) return; // Kontrola, zda existují UI elementy

        if (!isSlowed)
        {
            isSlowed = true;
            slowTimer = duration;
            slowIcon.gameObject.SetActive(true); // Zobrazí ikonu pro zpomalení
            StartCoroutine(SlowEffect());
            StartCoroutine(SlowTimer(duration));
        }
    }

    private IEnumerator PoisonDamage()
    {
        while (isPoisoned)
        {
            currentHealth -= 2; // Poison damage
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator BleedDamage()
    {
        while (isBleeding)
        {
            currentHealth -= 5; // Bleed damage
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator PoisonTimer(float duration)
    {
        while (duration > 0)
        {
            poisonTimerText.text = $"Poison: {Mathf.Ceil(duration)}s"; // Zobrazí èas otravy
            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }
        poisonIcon.gameObject.SetActive(false); // Skryje ikonu otravy po skonèení
        isPoisoned = false;
    }

    private IEnumerator BleedTimer(float duration)
    {
        while (duration > 0)
        {
            bleedTimerText.text = $"Bleed: {Mathf.Ceil(duration)}s"; // Zobrazí èas krvácení
            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }
        bleedIcon.gameObject.SetActive(false); // Skryje ikonu krvácení po skonèení
        isBleeding = false;
    }

    private IEnumerator SlowTimer(float duration)
    {
        while (duration > 0)
        {
            slowTimerText.text = $"Slow: {Mathf.Ceil(duration)}s"; // Zobrazí èas zpomalení
            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }
        slowIcon.gameObject.SetActive(false); // Skryje ikonu zpomalení po skonèení
        isSlowed = false;
    }

    private IEnumerator SlowEffect()
    {
        while (isSlowed)
        {
            // Implementace zpomalení pohybu hráèe (napø. snížení rychlosti)
            yield return null;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Odhlásí událost pøi znièení objektu
    }

    public void StopEffects()
    {
        isPoisoned = false;
        isBleeding = false;
        isSlowed = false;
    }
}
