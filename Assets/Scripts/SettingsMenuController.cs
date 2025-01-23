using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    public GameObject mainMenuPanel; // Panel hlavn�ho menu
    public GameObject settingsPanel; // Panel nastaven�
    public GameObject titlePanel;
    public GameObject infoPanel;     // Podpanel Info
    public GameObject audioPanel;    // Podpanel Audio
    public GameObject graphicsPanel; // Podpanel Graphics

    public Button infoButton;        // Tla��tko Info
    public Button audioButton;       // Tla��tko Audio
    public Button graphicsButton;    // Tla��tko Graphics

    public Color normalColor = Color.white; // Norm�ln� barva tla��tek
    public Color highlightedColor = Color.green; // Zv�razn�n� barva tla��tka

    private Button currentHighlightedButton; // Aktu�ln� zv�razn�n� tla��tko

    // Otev�en� nastaven� a skryt� hlavn�ho menu
    public void OpenSettings()
    {
        if (settingsPanel != null && mainMenuPanel != null && titlePanel != null)
        {
            settingsPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
            titlePanel.SetActive(false);
            ShowPanel("Info"); // V�choz� panel
        }
    }

    // Zav�en� nastaven� a n�vrat na hlavn� menu
    public void CloseSettings()
    {
        if (settingsPanel != null && mainMenuPanel != null && titlePanel != null)
        {
            settingsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            titlePanel.SetActive(true);
        }
    }

    // P�epnut� mezi jednotliv�mi panely nastaven�
    public void ShowPanel(string panelName)
    {
        // Skryje v�echny panely
        infoPanel.SetActive(false);
        audioPanel.SetActive(false);
        graphicsPanel.SetActive(false);

        // Resetuje barvy tla��tek
        ResetButtonColors();

        // Zobraz� odpov�daj�c� panel a zv�razn� odpov�daj�c� tla��tko
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

    // Zv�razn� tla��tko zm�nou barvy jeho obr�zku
    private void HighlightButton(Button button)
    {
        if (currentHighlightedButton != null)
        {
            // Resetuje barvu p�edchoz�ho tla��tka
            ResetButtonColor(currentHighlightedButton);
        }

        // Nastav� nov� tla��tko jako zv�razn�n�
        currentHighlightedButton = button;

        // Zm�n� barvu obr�zku tla��tka na zv�razn�nou
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = highlightedColor;
        }
    }

    // Resetuje barvy v�ech tla��tek na norm�ln�
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
