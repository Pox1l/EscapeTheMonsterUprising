using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private static GameOverManager instance;
    public GameObject gameOverPanel; // Pøipoj UI panel

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // UI pøežije zmìnu scény
        }
        else
        {
            Destroy(gameObject);
            return;
        }

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
    }

    public void RestartGame()
    {
        PlayerHealth.Instance.SetHealth(PlayerHealth.Instance.maxHealth); // Reset HP

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Skryje UI
        }

        SceneManager.LoadScene(1); // Naète první scénu
        SceneManager.sceneLoaded += OnSceneLoaded; // Po naètení nastaví hráèe
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Najdi EntryPoint ve scénì a pøesuò hráèe
        GameObject entryPoint = GameObject.Find("EntryPoint");
        if (entryPoint != null && PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.transform.position = entryPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("EntryPoint nebyl nalezen ve scénì!");
        }

        SceneManager.sceneLoaded -= OnSceneLoaded; // Odpojíme event, aby se nespouštìl znovu
    }

    public void QuitGame()
    {
        Application.Quit(); // Ukonèí hru
    }
}
