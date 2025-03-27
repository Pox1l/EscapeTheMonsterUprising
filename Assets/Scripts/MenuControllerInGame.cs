using UnityEngine;
using UnityEngine.UI;

public class MenuControllerInGamee : MonoBehaviour
{
    public GameObject settingsPanel; // Panel nastavení
    public GameObject infoPanel;
    public GameObject audioPanel;
    public GameObject graphicsPanel;

    public Button infoButton;
    public Button audioButton;
    public Button graphicsButton;

    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider uiSlider;

    public Color normalColor = Color.white;
    public Color highlightedColor = Color.green;

    private Button currentHighlightedButton;

    private void Start()
    {
        // Naèti uložené hodnoty hlasitosti
        if (AudioManager.instance != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
            uiSlider.value = PlayerPrefs.GetFloat("UIVolume", 0.5f);
        }

        // Pøidej event listenery pro slidery
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        uiSlider.onValueChanged.AddListener(SetUIVolume);
    }

    // Otevøe nastavení (funguje i v pauze)
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        ShowPanel("Audio"); // Otevøe pøímo zvukové nastavení
    }

    // Zavøe nastavení
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // Pøepnutí mezi jednotlivými panely
    public void ShowPanel(string panelName)
    {
        infoPanel.SetActive(false);
        audioPanel.SetActive(false);
        graphicsPanel.SetActive(false);

        ResetButtonColors();

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
                Debug.LogWarning("Neznámý panel: " + panelName);
                break;
        }
    }

    private void HighlightButton(Button button)
    {
        if (currentHighlightedButton != null)
        {
            ResetButtonColor(currentHighlightedButton);
        }

        currentHighlightedButton = button;
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = highlightedColor;
        }
    }

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

    // Aktualizace hlasitosti a uložení hodnoty
    public void SetMusicVolume(float volume)
    {
        AudioManager.instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.instance.SetSFXVolume(volume);
    }

    public void SetUIVolume(float volume)
    {
        AudioManager.instance.SetUIVolume(volume);
    }
}
