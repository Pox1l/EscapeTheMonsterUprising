using UnityEngine;
using System.IO;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance { get; private set; }

    [SerializeField] private int startingMoney = 100; // Poèáteèní peníze
    private int currentMoney;
    private string savePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Pøetrvá mezi scénami

        savePath = Path.Combine(Application.persistentDataPath, "moneyData.json");
        LoadMoney();
    }

    private void OnApplicationQuit()
    {
        SaveMoney();
    }

    public int GetMoney()
    {
        return currentMoney;
    }

    public bool HasEnoughMoney(int amount)
    {
        return currentMoney >= amount;
    }

    public void SpendMoney(int amount)
    {
        if (HasEnoughMoney(amount))
        {
            currentMoney -= amount;
            Debug.Log("Money spent: " + amount + ". Remaining: " + currentMoney);
            SaveMoney();
        }
        else
        {
            Debug.LogError("Not enough money!");
        }
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("Money added: " + amount + ". Total: " + currentMoney);
        SaveMoney();
    }

    private void SaveMoney()
    {
        string json = JsonUtility.ToJson(new MoneyData(currentMoney));
        File.WriteAllText(savePath, json);
        Debug.Log("Money saved to " + savePath);
    }

    private void LoadMoney()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            MoneyData data = JsonUtility.FromJson<MoneyData>(json);
            currentMoney = data.money;
        }
        else
        {
            currentMoney = startingMoney;
        }
        Debug.Log("Money loaded: " + currentMoney);
    }

    [System.Serializable]
    private class MoneyData
    {
        public int money;
        public MoneyData(int money) { this.money = money; }
    }
}
