using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/saveData.json";

    public static void SaveNPCCount(int count)
    {
        SaveData data = new SaveData { totalNPCs = count };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("Data uložena: " + json);
    }

    public static int LoadNPCCount()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data.totalNPCs;
        }
        return 0; // Pokud soubor neexistuje, vrátí 0
    }
}

[System.Serializable]
public class SaveData
{
    public int totalNPCs;
}
