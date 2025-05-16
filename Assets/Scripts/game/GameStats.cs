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
}
