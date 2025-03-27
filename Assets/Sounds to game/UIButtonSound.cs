using UnityEngine;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    public AudioClip buttonClickClip; // Mùžeš sem pøidat specifický zvuk pro tlaèítko, pokud chceš

    private void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => PlayButtonClickSound());
        }
    }

    private void PlayButtonClickSound()
    {
        if (AudioManager.instance != null)
        {
            // Pokud máš pøiøazený zvuk k tlaèítku
            if (buttonClickClip != null)
            {
                AudioManager.instance.PlayUISound(buttonClickClip);
            }
            else
            {
                // Pokud nemáš specifický zvuk, použije se defaultní zvuk
                AudioManager.instance.PlayUISound(AudioManager.instance.uiSource.clip);
            }
        }
    }
}
