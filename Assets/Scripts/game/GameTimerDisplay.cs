using UnityEngine;
using TMPro; // <- Dùležité pro TextMeshPro
using System;

public class GameTimerDisplay : MonoBehaviour
{
    public TMP_Text timerText;

    void Update()
    {
        if (GameTimerManager.Instance == null || timerText == null) return;

        float totalSeconds = GameTimerManager.Instance.GetTotalTimePlayed();
        TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        timerText.text =  formattedTime;
    }
}
