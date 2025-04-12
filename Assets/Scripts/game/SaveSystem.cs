using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/saveData.json";

    // Metoda pro uložení poètu zachránìných NPC
    public static void SaveNPCCount(int count)
    {
        SaveData data = new SaveData { totalNPCs = count };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("Data uložena: " + json);
    }

    // Metoda pro naètení poètu zachránìných NPC
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

    // Metoda pro pøidání NPC (zvýšení poètu)
    public static void AddRescuedNPCs(int countToAdd)
    {
        int currentNPCCount = LoadNPCCount(); // Naète aktuální poèet NPC
        int newCount = currentNPCCount + countToAdd; // Zvýší poèet NPC o požadovanou hodnotu
        SaveNPCCount(newCount); // Uloží nový poèet NPC
        Debug.Log("Zachránìné NPC byly zvýšeny o " + countToAdd + ". Nový poèet: " + newCount);
    }
}

[System.Serializable]
public class SaveData
{
    public int totalNPCs; // Poèet zachránìných NPC
}
