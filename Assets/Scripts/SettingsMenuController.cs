using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    public GameObject mainMenuPanel; // Panel hlavního menu
    public GameObject settingsPanel; // Panel nastavení
    public GameObject titlePanel;
    public GameObject infoPanel;     // Podpanel Info
    public GameObject audioPanel;    // Podpanel Audio
    public GameObject graphicsPanel; // Podpanel Graphics

    public Button infoButton;        // Tlaèítko Info
    public Button audioButton;       // Tlaèítko Audio
    public Button graphicsButton;    // Tlaèítko Graphics

    public Color normalColor = Color.white; // Normální barva tlaèítek
    public Color highlightedColor = Color.green; // Zvýraznìná barva tlaèítka

    private Button currentHighlightedButton; // Aktuálnì zvýraznìné tlaèítko

    // Otevøení nastavení a skrytí hlavního menu
    public void OpenSettings()
    {
        if (settingsPanel != null && mainMenuPanel != null && titlePanel != null)
        {
            settingsPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
            titlePanel.SetActive(false);
            ShowPanel("Info"); // Výchozí panel
        }
    }

    // Zavøení nastavení a návrat na hlavní menu
    public void CloseSettings()
    {
        if (settingsPanel != null && mainMenuPanel != null && titlePanel != null)
        {
            settingsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            titlePanel.SetActive(true);
        }
    }

    // Pøepnutí mezi jednotlivými panely nastavení
    public void ShowPanel(string panelName)
    {
        // Skryje všechny panely
        infoPanel.SetActive(false);
        audioPanel.SetActive(false);
        graphicsPanel.SetActive(false);

        // Resetuje barvy tlaèítek
        ResetButtonColors();

        // Zobrazí odpovídající panel a zvýrazní odpovídající tlaèítko
        switch (panelName)
        {
            case "Info":
                infoPanel.SetActive(true);
                HighlightButton(infoButton);
                break;
            case "Audio":
                audioPanel.SetActive(true);
                HighlightButton(audioButton);
                break;
            case "Graphics":
                graphicsPanel.SetActive(true);
                HighlightButton(graphicsButton);
                break;
            default:
                Debug.LogWarning("Unknown panel: " + panelName);
                break;
        }
    }

    // Zvýrazní tlaèítko zmìnou barvy jeho obrázku
    private void HighlightButton(Button button)
    {
        if (currentHighlightedButton != null)
        {
            // Resetuje barvu pøedchozího tlaèítka
            ResetButtonColor(currentHighlightedButton);
        }

        // Nastaví nové tlaèítko jako zvýraznìné
        currentHighlightedButton = button;

        // Zmìní barvu obrázku tlaèítka na zvýraznìnou
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = highlightedColor;
        }
    }

    // Resetuje barvy všech tlaèítek na normální
    private void ResetButtonColors()
    {
        ResetButtonColor(infoButton);
        ResetButtonColor(audioButton);
        ResetButtonColor(graphicsButton);
    }

    private void ResetButtonColor(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }
}
