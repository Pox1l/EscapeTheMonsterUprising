using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadVolumeSettings();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();  
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();  
    }

    public void SetUIVolume(float volume)
    {
        uiSource.volume = volume;
        PlayerPrefs.SetFloat("UIVolume", volume);
        PlayerPrefs.Save();  
    }

    private void LoadVolumeSettings()
    {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        uiSource.volume = PlayerPrefs.GetFloat("UIVolume", 0.5f);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayUISound(AudioClip clip)
    {
        if (clip != null)
            uiSource.PlayOneShot(clip);
    }
}
