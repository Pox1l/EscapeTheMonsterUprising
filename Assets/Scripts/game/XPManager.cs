using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    public static XPManager instance; // Singleton instance

    private Slider xpSlider;
    private int currentXP = 0;
    private int level = 1;
    private int xpToLevelUp = 100;
    private bool isOutdoorScene; // Funguje jen ve venkovní scénì

    public GameObject upgradeUIPanel; // UI panel pro upgrady

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        CheckScene();
    }

    public void AddXP(int amount)
    {
        if (!isOutdoorScene) return;

        currentXP += amount;
        Debug.Log($"Hráè sebral XP: {amount}, Aktuální XP: {currentXP}/{xpToLevelUp}, Level: {level}");

        if (currentXP >= xpToLevelUp)
        {
            currentXP = 0;
            level++;
            Debug.Log($"LEVEL UP! Nový level: {level}");
            ShowUpgradeUI(); // Zobrazí UI upgradu
        }

        UpdateUI();
    }

    private void ShowUpgradeUI()
    {
        if (upgradeUIPanel != null)
        {
            upgradeUIPanel.SetActive(true); // Aktivuje UI s upgrady
            Time.timeScale = 0f; // Pauzne hru, aby hráè vybral upgrade
        }
        else
        {
            Debug.LogWarning("Upgrade UI Panel není nastaven!");
        }
    }

    public void CloseUpgradeUI()
    {
        if (upgradeUIPanel != null)
        {
            upgradeUIPanel.SetActive(false);
            Time.timeScale = 1f; // Obnoví èas ve høe
        }
    }

    private void UpdateUI()
    {
        if (xpSlider != null)
        {
            xpSlider.value = (float)currentXP / xpToLevelUp;
        }
        else
        {
            Debug.LogWarning("XP Slider není nalezen!");
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
            xpSlider = GameObject.Find("XP_Slider")?.GetComponent<Slider>();

            GameObject upgradeUIObj = GameObject.Find("UpgradeUIPanel"); // Najde UI upgrade panel ve scénì
            if (upgradeUIObj != null)
                upgradeUIPanel = upgradeUIObj;

            UpdateUI();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
