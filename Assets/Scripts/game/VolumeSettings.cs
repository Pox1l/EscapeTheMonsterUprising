using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;

    private const string VolumeKey = "MusicVolume"; // Klíè pro uložení hlasitosti

    private void Start()
    {
        // Naètení uložené hodnoty, pokud existuje, jinak výchozí hodnota 10
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 10f);
        musicSlider.value = savedVolume;
        SetMusicVolume(); 

        // Pøidání listeneru pro automatické ukládání pøi zmìnì slideru
        musicSlider.onValueChanged.AddListener(delegate { SaveVolume(); });
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;

        if (volume == 0)
        {
            audioMixer.SetFloat("music", -80f); // Úplné ztlumení
        }
        else
        {
            float newVolume = Mathf.Lerp(-80f, 10f, volume / 20f); // Interpolace mezi -80 dB (ticho) a 10 dB (hlasitìjší hudba)
            audioMixer.SetFloat("music", newVolume);
        }
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat(VolumeKey, musicSlider.value); // Uloží hodnotu
        PlayerPrefs.Save(); // Zajistí uložení dat
        SetMusicVolume(); // Aktualizuje hlasitost
    }
}
