using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance; // Singleton instance

    public int maxHealth = 100; // Maxim�ln� HP
    private int currentHealth; // Aktu�ln� HP

    private TextMeshProUGUI healthText; // TextMeshPro pro zobrazen� HP
    private Slider healthSlider; // Slider pro vizu�ln� zobrazen� HP

    // UI pro zobrazen� efekt�
    public RawImage poisonIcon; // Ikona pro otravu
    public RawImage bleedIcon; // Ikona pro krv�cen�
    public RawImage slowIcon; // Ikona pro zpomalen�
    public TextMeshProUGUI poisonTimerText; // Text pro odpo��t�v�n� otravy
    public TextMeshProUGUI bleedTimerText; // Text pro odpo��t�v�n� krv�cen�
    public TextMeshProUGUI slowTimerText; // Text pro odpo��t�v�n� zpomalen�

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

        GameObject healthSliderObject = GameObject.Find("HealthSlider");
        if (healthSliderObject != null)
        {
            healthSlider = healthSliderObject.GetComponent<Slider>();
            Debug.Log("healthSlider byl nalezen a p�i�azen!");

            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("HealthSlider objekt nebyl nalezen ve sc�n�!");
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
            Debug.LogWarning("healthText nen� p�i�azen!");
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("healthSlider nen� p�i�azen!");
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
    }

    public void ApplyPoisonEffect(float duration)
    {
        if (poisonIcon == null || poisonTimerText == null) return; // Kontrola, zda existuj� UI elementy

        if (!isPoisoned)
        {
            isPoisoned = true;
            poisonTimer = duration;
            poisonIcon.gameObject.SetActive(true); // Zobraz� ikonu pro otravu
            StartCoroutine(PoisonDamage());
            StartCoroutine(PoisonTimer(duration));
        }
    }

    public void ApplyBleedEffect(float duration)
    {
        if (bleedIcon == null || bleedTimerText == null) return; // Kontrola, zda existuj� UI elementy

        if (!isBleeding)
        {
            isBleeding = true;
            bleedTimer = duration;
            bleedIcon.gameObject.SetActive(true); // Zobraz� ikonu pro krv�cen�
            StartCoroutine(BleedDamage());
            StartCoroutine(BleedTimer(duration));
        }
    }

    public void ApplySlowEffect(float duration)
    {
        if (slowIcon == null || slowTimerText == null) return; // Kontrola, zda existuj� UI elementy

        if (!isSlowed)
        {
            isSlowed = true;
            slowTimer = duration;
            slowIcon.gameObject.SetActive(true); // Zobraz� ikonu pro zpomalen�
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
            poisonTimerText.text = $"Poison: {Mathf.Ceil(duration)}s"; // Zobraz� �as otravy
            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }
        poisonIcon.gameObject.SetActive(false); // Skryje ikonu otravy po skon�en�
        isPoisoned = false;
    }

    private IEnumerator BleedTimer(float duration)
    {
        while (duration > 0)
        {
            bleedTimerText.text = $"Bleed: {Mathf.Ceil(duration)}s"; // Zobraz� �as krv�cen�
            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }
        bleedIcon.gameObject.SetActive(false); // Skryje ikonu krv�cen� po skon�en�
        isBleeding = false;
    }

    private IEnumerator SlowTimer(float duration)
    {
        while (duration > 0)
        {
            slowTimerText.text = $"Slow: {Mathf.Ceil(duration)}s"; // Zobraz� �as zpomalen�
            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }
        slowIcon.gameObject.SetActive(false); // Skryje ikonu zpomalen� po skon�en�
        isSlowed = false;
    }

    private IEnumerator SlowEffect()
    {
        while (isSlowed)
        {
            // Implementace zpomalen� pohybu hr��e (nap�. sn�en� rychlosti)
            yield return null;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Odhl�s� ud�lost p�i zni�en� objektu
    }

    public void StopEffects()
    {
        isPoisoned = false;
        isBleeding = false;
        isSlowed = false;
    }
}
