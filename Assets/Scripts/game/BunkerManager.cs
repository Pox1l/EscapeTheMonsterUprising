using UnityEngine;
using TMPro; // Ujisti se, �e pou��v� TextMeshPro pro texty
using UnityEngine.SceneManagement; // Nezapome� na import pro SceneManager

public class BunkerManager : MonoBehaviour
{
    public GameObject resultsTableUI; // Reference na UI objekt s v�sledky
    public TextMeshProUGUI moneyText; // Text pro zobrazen� pen�z
    public TextMeshProUGUI xpText; // Text pro zobrazen� XP
    public TextMeshProUGUI npcText; // Text pro zobrazen� po�tu zachr�n�n�ch NPC

    private void Start()
    {
        // Zkontroluj, zda jsme ve sc�n� 1, a pokud ano, zobraz tabulku
        if (SceneManager.GetActiveScene().buildIndex == 1) // nebo pou�ij index: SceneManager.GetActiveScene().buildIndex == 1
        {
            if (HasValidResults())
            {
                ShowResultsTable();
            }
        }
    }

    private bool HasValidResults()
    {
        // Podm�nka pro zobrazen� tabulky
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
            Debug.LogError("Results Table UI nen� p�i�azen v BunkerManageru!");
        }
    }

    private void UpdateResultsTable()
    {
        // Aktualizace textov�ch hodnot na tabulce
        if (moneyText != null && xpText != null && npcText != null)
        {
            moneyText.text = $"Pen�ze: {GameManager.Instance.totalMoney}";
            xpText.text = $"XP: {GameManager.Instance.totalXP}";
            npcText.text = $"Zachr�n�no NPC: {GameManager.Instance.rescuedNPCCount}";
        }
        else
        {
            Debug.LogError("Chyb� n�kter� z textov�ch objekt� (moneyText, xpText, npcText)!");
        }
    }

    // Funkce pro zav�en� tabulky
    public void CloseResultsTable()
    {
        if (resultsTableUI != null)
        {
            resultsTableUI.SetActive(false); // Skryje tabulku
        }
    }

    private void Update()
    {
        // Mo�nost zav��t tabulku tla��tkem (nap�. Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseResultsTable(); // Zav�e tabulku, kdy� stiskne� Escape
        }
    }
}
