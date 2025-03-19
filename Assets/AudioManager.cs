
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-------------------------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("-------------------------------")]
    public AudioClip background;
    public AudioClip xpSound;
    public AudioClip buttonClick;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
