using UnityEngine;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    private void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => AudioManager.instance.PlayUISound(AudioManager.instance.uiSource.clip));
        }
    }
}
