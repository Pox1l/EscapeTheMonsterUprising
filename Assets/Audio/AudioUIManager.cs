using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioUIManager : MonoBehaviour
{
    public static AudioUIManager instance;

    public AudioSource uiSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Necháváš ho znièit pøi zmìnì scény, což je správnì pro scénové UI
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadVolumeSettings();
    }
    public void SetUIVolume(float volume)
    {
        uiSource.volume = volume;
        PlayerPrefs.SetFloat("UIVolume", volume);
        PlayerPrefs.Save();
    }
    private void LoadVolumeSettings()
    {
        uiSource.volume = PlayerPrefs.GetFloat("UIVolume", 0.5f);
    }
    public void PlayUISound(AudioClip clip)
    {
        if (clip != null)
            uiSource.PlayOneShot(clip);
    }
}
