using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;

    public AudioClip gunShotClip;
    public AudioClip npcRescueClip;
    public AudioClip xpPickupClip;
    public AudioClip damageClip;
    public AudioClip deadClip;
    public AudioClip healClip;
    public AudioClip closeDoorClip;
    public AudioClip openDoorClip;


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

    public void PlayGunShot()
    {
        PlaySFX(gunShotClip);
    }

    public void PlayNPCRescue()
    {
        PlaySFX(npcRescueClip);
    }

    public void PlayXPPickup()
    {
        PlaySFX(xpPickupClip);
    }
    public void DamageClip()
    {
        PlaySFX(damageClip);
    }
    public void DeadClip()
    {
        PlaySFX(deadClip);
    }
    public void HealClip()
    {
        PlaySFX(healClip);
    }
    public void CloseDoor()
    {
        PlaySFX(closeDoorClip);
    }
    public void OpenDoor()
    {
        PlaySFX(openDoorClip);
    }

}
