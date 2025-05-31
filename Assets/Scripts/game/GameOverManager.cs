using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public int moneyLossOnDeath = 50;

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("GameOverPanel není nastaveno v GameOverManager!");
        }
    }

    public void ShowGameOverUI()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    private void LoseMoneyOnDeath()
    {
        if (PlayerMoney.Instance != null)
        {
            PlayerMoney.Instance.SpendMoney(moneyLossOnDeath);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.Respawn(); // Správný respawn
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(1);

        LoseMoneyOnDeath();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject entryPoint = GameObject.Find("EntryPoint");
        if (entryPoint != null && PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.transform.position = entryPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("EntryPoint nebyl nalezen ve scénì!");
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void QuitGame()
    {
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.SetHealth(PlayerHealth.Instance.maxHealth);
        }

        Application.Quit();
    }
}
