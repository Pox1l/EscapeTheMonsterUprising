using UnityEngine;

public class GameTimerManager : MonoBehaviour
{
    public static GameTimerManager Instance;

    private float sessionTime = 0f;
    private float totalTimePlayed = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        totalTimePlayed = PlayerPrefs.GetFloat("TotalTimePlayed", 0f);
    }

    void Update()
    {
        sessionTime += Time.unscaledDeltaTime; // <- tady je zmìna
    }

    void OnApplicationQuit()
    {
        SaveTotalTime();
    }

    void OnDisable()
    {
        SaveTotalTime();
    }

    private void SaveTotalTime()
    {
        PlayerPrefs.SetFloat("TotalTimePlayed", totalTimePlayed + sessionTime);
        PlayerPrefs.Save();
    }

    public float GetTotalTimePlayed()
    {
        return totalTimePlayed + sessionTime;
    }
}
