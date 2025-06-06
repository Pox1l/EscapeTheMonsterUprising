using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider uiSlider;

    private void Start()
    {
        // Naètení uložených hodnot pøi spuštìní
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        uiSlider.value = PlayerPrefs.GetFloat("UIVolume", 0.5f);

        // Pøidání listenerù pro zmìnu hlasitosti
        musicSlider.onValueChanged.AddListener(AudioManager.instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
        uiSlider.onValueChanged.AddListener(AudioUIManager.instance.SetUIVolume);
    }
}
