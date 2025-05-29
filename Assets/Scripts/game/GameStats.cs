using UnityEngine;
using TMPro; // Pøidání TextMeshPro knihovny

public class GameStats : MonoBehaviour
{
    public GameObject uiPanel; // UI panel pro zobrazení statistik
    public TextMeshProUGUI rescuedNPCText; // TextMeshPro prvek pro zobrazení poètu NPC

    private bool isPanelActive = false;

    private void Start()
    {
        UpdateRescuedNPCText(); // Aktualizace textu pøi spuštìní
    }

    private void Update()
    {
        // Ovládání UI panelu klávesou Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUIPanel();
        }
    }

    private void ToggleUIPanel()
    {
        if (uiPanel != null)
        {
            isPanelActive = !isPanelActive;
            uiPanel.SetActive(isPanelActive);
            Time.timeScale = isPanelActive ? 0f : 1f; // Pozastavení nebo obnovení èasu

            if (isPanelActive)
            {
                UpdateRescuedNPCText(); // Aktualizovat text pøi otevøení panelu
                SearchForPlayer();      // Hledání objektu s tagem "Player"

                // Volání metod Player_XP pøi otevøení
                Player_XP.Instance.FindXPUI();
                Player_XP.Instance.UpdateXPUI();
            }
        }
    }

    public void CloseUIPanel()
    {
        if (uiPanel != null)
        {
            isPanelActive = false;
            uiPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void UpdateRescuedNPCText()
    {
        if (rescuedNPCText != null)
        {
            int rescuedNPCCount = SaveSystem.LoadNPCCount(); // Naètení z JSON
            rescuedNPCText.text = $"NPC saved: {rescuedNPCCount}";
        }
    }

    // Nová funkce, která hledá objekt s tagem "Player"
    private void SearchForPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Debug.Log("Player nalezen: " + player.name);
            // Další logika zde
        }
        else
        {
            Debug.Log("Player nebyl nalezen.");
        }
    }
}
