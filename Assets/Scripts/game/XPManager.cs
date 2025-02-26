using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    public static XPManager instance; // Singleton instance

    private Slider xpSlider;
    private Text levelText;
    private int currentXP = 0;
    private int level = 1;
    private int xpToLevelUp = 100;

    private bool isOutdoorScene; // Funguje jen ve venkovní scénì

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Zachová objekt mezi scénami
        }
        else
        {
            Destroy(gameObject); // Zabrání duplikaci
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        CheckScene();
    }

    public void AddXP(int amount)
    {
        if (!isOutdoorScene) return; // XP funguje jen venku

        currentXP += amount;
        Debug.Log($"Hráè sebral XP: {amount}, Aktuální XP: {currentXP}/{xpToLevelUp}, Level: {level}");

        if (currentXP >= xpToLevelUp)
        {
            currentXP = 0;
            level++;
            Debug.Log($"LEVEL UP! Nový level: {level}");
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (xpSlider != null)
        {
            xpSlider.value = (float)currentXP / xpToLevelUp;
            Debug.Log($"Aktualizace slideru: {xpSlider.value * 100}% XP");
        }
        else
        {
            Debug.LogWarning("XP Slider není nalezen!");
        }

        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }
        else
        {
            Debug.LogWarning("Level Text není nalezen!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckScene();
    }

    private void CheckScene()
    {
        isOutdoorScene = SceneManager.GetActiveScene().buildIndex == 2; // ID venkovní scény

        if (isOutdoorScene)
        {
            //Debug.Log("XP systém aktivní! (Venku)");
            xpSlider = GameObject.Find("XP_Slider")?.GetComponent<Slider>();
            levelText = GameObject.Find("Level_Text")?.GetComponent<Text>();
            UpdateUI();
        }
        else
        {
            //Debug.Log("XP systém vypnutý! (Bunkr)");
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
