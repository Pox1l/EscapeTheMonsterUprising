using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenEffectController : MonoBehaviour
{
    public static ScreenEffectController Instance;

    public Image damageOverlay;
    public Image healOverlay;

    public float fadeDuration = 0.3f;
    public float maxAlpha = 0.4f;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();

        // Napø. ignoruj scény menu, kde nejsou overlaye
        if (scene.name == "Menu" || scene.buildIndex == 0)
        {
            damageOverlay = null;
            healOverlay = null;
            return;
        }

        damageOverlay = GameObject.Find("DamageOverlay")?.GetComponent<Image>();
        healOverlay = GameObject.Find("HealOverlay")?.GetComponent<Image>();

        if (damageOverlay == null)
            Debug.LogWarning("DamageOverlay nebyl nalezen ve scénì!");
        if (healOverlay == null)
            Debug.LogWarning("HealOverlay nebyl nalezen ve scénì!");
    }


    public void PlayDamageEffect()
    {
        if (damageOverlay != null)
            StartCoroutine(FadeOverlay(damageOverlay));
    }

    public void PlayHealEffect()
    {
        if (healOverlay != null)
            StartCoroutine(FadeOverlay(healOverlay));
    }

    private IEnumerator FadeOverlay(Image overlay)
    {
        // Fade in
        float t = 0;
        while (t < fadeDuration)
        {
            if (overlay == null)
                yield break;

            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0, maxAlpha, t / fadeDuration);
            SetAlpha(overlay, alpha);
            yield return null;
        }

        // Fade out
        t = 0;
        while (t < fadeDuration)
        {
            if (overlay == null)
                yield break;

            t += Time.deltaTime;
            float alpha = Mathf.Lerp(maxAlpha, 0, t / fadeDuration);
            SetAlpha(overlay, alpha);
            yield return null;
        }

        if (overlay != null)
            SetAlpha(overlay, 0);
    }

    private void SetAlpha(Image img, float alpha)
    {
        if (img == null)
            return;

        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}

