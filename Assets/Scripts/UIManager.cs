using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI remainingEnemyText;  // Text pro poèet zbývajících nepøátel

    private static UIManager instance;

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
        }
    }

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    Debug.LogError("UIManager nebyl nalezen ve scénì!");
                }
            }
            return instance;
        }
    }

    public void FindUIElements()
    {
        waveText = GameObject.Find("WaveText")?.GetComponent<TextMeshProUGUI>();
        enemyCountText = GameObject.Find("EnemyCountText")?.GetComponent<TextMeshProUGUI>();
        remainingEnemyText = GameObject.Find("RemainingEnemyText")?.GetComponent<TextMeshProUGUI>(); // Nový text pro zbývající nepøátele

        if (waveText == null || enemyCountText == null || remainingEnemyText == null)
        {
            Debug.LogError("UI textové prvky nebyly nalezeny! Ujisti se, že mají správné názvy.");
        }
        else
        {
            Debug.Log("UI textové prvky byly nalezeny.");
        }
    }

    public void ShowWave(int waveNumber, int enemyCount)
    {
        if (waveText == null || enemyCountText == null || remainingEnemyText == null)
        {
            FindUIElements();
        }

        // Debug log pro kontrolu, zda se hodnoty aktualizují správnì
        Debug.Log($"Aktualizuji UI: Vlna {waveNumber}, Nepøátelé {enemyCount}");

        waveText.text = "Wave: " + waveNumber;
        enemyCountText.text = "Enemy: " + enemyCount;
        remainingEnemyText.text = "Remaining: " + enemyCount;  // Nastavujeme i poèet zbývajících nepøátel

        waveText.gameObject.SetActive(true);
        enemyCountText.gameObject.SetActive(true);
        remainingEnemyText.gameObject.SetActive(true);  // Ukazujeme i nový text

        // Zaèneme skript pro skrytí wave textu
        StartCoroutine(HideWaveText());
    }

    private IEnumerator HideWaveText()
    {
        yield return new WaitForSeconds(3f); // Poèká 3 sekundy
        waveText.gameObject.SetActive(false);
        enemyCountText.gameObject.SetActive(false); // Skrýváme i count text
    }

    public void SetWaveUIVisibility(bool visible)
    {
        if (waveText == null || enemyCountText == null || remainingEnemyText == null)
        {
            FindUIElements();
        }

        waveText.gameObject.SetActive(visible);
        enemyCountText.gameObject.SetActive(visible);
        remainingEnemyText.gameObject.SetActive(visible); // Kontrola viditelnosti pro text o zbývajících nepøátelích
    }

    // Metoda pro aktualizaci zbývajících nepøátel
    public void UpdateRemainingEnemies(int remainingEnemies)
    {
        if (remainingEnemyText != null)
        {
            remainingEnemyText.text = "Remaining: " + remainingEnemies;
        }
    }
}
