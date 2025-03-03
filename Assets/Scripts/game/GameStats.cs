using UnityEngine;

public class GameStats : MonoBehaviour
{
    public GameObject uiPanel; // UI panel pro zobrazení statistik
    private bool isPanelActive = false;

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
        }
    }
}