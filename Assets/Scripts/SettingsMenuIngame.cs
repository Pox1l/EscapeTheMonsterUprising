using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuIngamee : MonoBehaviour
{
    [Header("Sub Panels")]
    public GameObject infoPanel;
    public GameObject audioPanel;
    public GameObject graphicsPanel;

    [Header("Buttons")]
    public Button infoButton;
    public Button audioButton;
    public Button graphicsButton;

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
            { "Info", infoPanel },
            { "Audio", audioPanel },
            { "Graphics", graphicsPanel }
        };

        // Nastavíme výchozí panel
        ShowPanel("Info");

        // Pøipojíme tlaèítka k jejich funkcím
        infoButton.onClick.AddListener(() => ShowPanel("Info"));
        audioButton.onClick.AddListener(() => ShowPanel("Audio"));
        graphicsButton.onClick.AddListener(() => ShowPanel("Graphics"));
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
            "Info" => infoButton,
            "Audio" => audioButton,
            "Graphics" => graphicsButton,
            _ => null
        };
    }
}
