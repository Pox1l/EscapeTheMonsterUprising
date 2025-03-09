using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/save.json";

    [System.Serializable]
    private class SaveData
    {
        public int rescuedNPCCount;
    }

    public static void SaveGame(int rescuedNPCCount)
    {
        SaveData data = new SaveData { rescuedNPCCount = rescuedNPCCount };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public static int LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data.rescuedNPCCount;
        }
        return 0;
    }
}
