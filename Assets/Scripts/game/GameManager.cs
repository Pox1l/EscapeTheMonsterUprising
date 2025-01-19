using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Statistické hodnoty
    public int totalMoney { get; private set; }
    public int totalXP { get; private set; }
    public int rescuedNPCCount { get; private set; }
    public string currentWeaponName { get; private set; }

    private const int moneyPerNPC = 50;
    private const int xpPerNPC = 10;
    private string filePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
    }

    private void Start()
    {
        LoadPlayerData();
    }

    public void AddMoney(int amount)
    {
        totalMoney += amount;
        Debug.Log($"Peníze pøidány: {amount}. Celkový zùstatek: {totalMoney}");
    }

    public void AddXP(int amount)
    {
        totalXP += amount;
        Debug.Log($"XP pøidáno: {amount}. Celkové XP: {totalXP}");
    }

    public void IncrementRescuedNPCCount()
    {
        rescuedNPCCount++;
        AddMoney(moneyPerNPC);
        AddXP(xpPerNPC);
        Debug.Log($"NPC zachránìno: {rescuedNPCCount}. Peníze: {totalMoney}, XP: {totalXP}");
    }

    public void SetCurrentWeapon(string weaponName)
    {
        currentWeaponName = weaponName;
    }

    public void SavePlayerData()
    {
        PlayerData data = new PlayerData
        {
            money = totalMoney,
            xp = totalXP,
            rescuedNPCCount = rescuedNPCCount,
            currentWeaponName = currentWeaponName
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);

        Debug.Log("Data saved to " + filePath);
    }

    public void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            totalMoney = data.money;
            totalXP = data.xp;
            rescuedNPCCount = data.rescuedNPCCount;
            currentWeaponName = data.currentWeaponName;

            Debug.Log("Data loaded from " + filePath);
        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }
}
