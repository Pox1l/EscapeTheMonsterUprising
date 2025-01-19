using UnityEngine;
using TMPro; // Ujisti se, že používáš TextMeshPro pro texty
using UnityEngine.SceneManagement; // Nezapomeò na import pro SceneManager

public class BunkerManager : MonoBehaviour
{
    public GameObject resultsTableUI; // Reference na UI objekt s výsledky
    public TextMeshProUGUI moneyText; // Text pro zobrazení penìz
    public TextMeshProUGUI xpText; // Text pro zobrazení XP
    public TextMeshProUGUI npcText; // Text pro zobrazení poètu zachránìných NPC

    private void Start()
    {
        // Zkontroluj, zda jsme ve scénì 1, a pokud ano, zobraz tabulku
        if (SceneManager.GetActiveScene().buildIndex == 1) // nebo použij index: SceneManager.GetActiveScene().buildIndex == 1
        {
            if (HasValidResults())
            {
                ShowResultsTable();
            }
        }
    }

    private bool HasValidResults()
    {
        // Podmínka pro zobrazení tabulky
        return GameManager.Instance.totalMoney > 0 || GameManager.Instance.totalXP > 0 || GameManager.Instance.rescuedNPCCount > 0;
    }

    private void ShowResultsTable()
    {
        // Aktivuj tabulku
        if (resultsTableUI != null)
        {
            resultsTableUI.SetActive(true);
            UpdateResultsTable();
        }
        else
        {
            Debug.LogError("Results Table UI není pøiøazen v BunkerManageru!");
        }
    }

    private void UpdateResultsTable()
    {
        // Aktualizace textových hodnot na tabulce
        if (moneyText != null && xpText != null && npcText != null)
        {
            moneyText.text = $"Peníze: {GameManager.Instance.totalMoney}";
            xpText.text = $"XP: {GameManager.Instance.totalXP}";
            npcText.text = $"Zachránìno NPC: {GameManager.Instance.rescuedNPCCount}";
        }
        else
        {
            Debug.LogError("Chybí nìkterý z textových objektù (moneyText, xpText, npcText)!");
        }
    }

    // Funkce pro zavøení tabulky
    public void CloseResultsTable()
    {
        if (resultsTableUI != null)
        {
            resultsTableUI.SetActive(false); // Skryje tabulku
        }
    }

    private void Update()
    {
        // Možnost zavøít tabulku tlaèítkem (napø. Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseResultsTable(); // Zavøe tabulku, když stiskneš Escape
        }
    }
}
