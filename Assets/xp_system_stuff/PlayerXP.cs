using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerXP : MonoBehaviour
{
    private Slider xpSlider;
    private Text levelText;
    private int currentXP;
    private int level = 1;
    private int xpToLevelUp = 100;

    void Start()
    {
        if (!IsOutdoorScene()) return;

        xpSlider = GameObject.Find("XP_Slider")?.GetComponent<Slider>();
        levelText = GameObject.Find("Level_Text")?.GetComponent<Text>();

        if (xpSlider == null || levelText == null)
        {
            Debug.LogWarning("Slider nebo Text nenalezeny!");
        }

        UpdateUI();
    }

    public void AddXP(int amount)
    {
        if (!IsOutdoorScene()) return;

        currentXP += amount;
        if (currentXP >= xpToLevelUp)
        {
            currentXP = 0;
            level++;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (xpSlider != null) xpSlider.value = (float)currentXP / xpToLevelUp;
        if (levelText != null) levelText.text = "Level: " + level;
    }

    private bool IsOutdoorScene()
    {
        return SceneManager.GetActiveScene().buildIndex == 2; // Upravit podle ID venkovní scény
    }
}
