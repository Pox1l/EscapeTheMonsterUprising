using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Pøipoj UI panel
    public int moneyLossOnDeath = 50; // Poèet penìz, které hráè ztratí pøi smrti

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Na zaèátku skryté
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

        Time.timeScale = 0f; // Zastaví èas ve høe
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
        Time.timeScale = 1f; // Obnoví èas

        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.SetHealth(PlayerHealth.Instance.maxHealth); // Reset HP
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Skryje UI
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(1); // Naète první scénu

        LoseMoneyOnDeath(); // Odebere peníze po smrti
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
            PlayerHealth.Instance.SetHealth(PlayerHealth.Instance.maxHealth); // Nastaví HP na max
        }
        Application.Quit(); // Ukonèí hru
    }
}
