using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuIngamee : MonoBehaviour
{
    [Header("Sub Panels")]
    public GameObject controlsPanel;
    public GameObject audioPanel;
    public GameObject infoPanel;

    [Header("Buttons")]
    public Button controlsButton;
    public Button audioButton;
    public Button infoButton;
    public Button closeButton;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color highlightedColor = Color.green;

    private Button currentHighlightedButton;
    private Dictionary<string, GameObject> panels;

    void Start()
    {
        // Vytvoøíme slovník panelù
        panels = new Dictionary<string, GameObject>
        {
            { "Controls", controlsPanel },
            { "Audio", audioPanel },
            { "Info", infoPanel }
        };

        // Nastavíme výchozí panel
        ShowPanel("Controls");

        // Pøipojíme tlaèítka k jejich funkcím
        controlsButton.onClick.AddListener(() => ShowPanel("Controls"));
        audioButton.onClick.AddListener(() => ShowPanel("Audio"));
        infoButton.onClick.AddListener(() => ShowPanel("Info"));
        closeButton.onClick.AddListener(CloseSettings);
    }

    // Pøepínání mezi panely
    public void ShowPanel(string panelName)
    {
        // Skryje všechny panely
        foreach (var panel in panels.Values)
        {
            panel.SetActive(false);
        }

        // Aktivuje požadovaný panel
        if (panels.ContainsKey(panelName))
        {
            panels[panelName].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Unknown panel: " + panelName);
            return;
        }

        // Zvýrazní odpovídající tlaèítko
        HighlightButton(GetButtonByName(panelName));
    }

    private void HighlightButton(Button button)
    {
        if (currentHighlightedButton != null)
        {
            ResetButtonColor(currentHighlightedButton);
        }

        currentHighlightedButton = button;
        if (button != null)
        {
            button.GetComponent<Image>().color = highlightedColor;
        }
    }

    private void ResetButtonColor(Button button)
    {
        if (button != null)
        {
            button.GetComponent<Image>().color = normalColor;
        }
    }

    private Button GetButtonByName(string panelName)
    {
        return panelName switch
        {
            "Controls" => controlsButton,
            "Audio" => audioButton,
            "Info" => infoButton,
            _ => null
        };
    }

    public void CloseSettings()
    {
        // Najde PauseMenu skript ve scénì a zavolá jeho CloseSettings
        PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu != null)
        {
            pauseMenu.CloseSettings();
        }
        else
        {
            Debug.LogWarning("PauseMenu not found in scene.");
        }
    }

}
