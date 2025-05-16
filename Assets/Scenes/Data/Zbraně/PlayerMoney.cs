using UnityEngine;
using TMPro;
using System.IO;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance { get; private set; }

    [SerializeField] private int startingMoney = 100; // Poèáteèní peníze
    private int currentMoney;
    private string savePath;

    [SerializeField]
    private TextMeshProUGUI moneyText; // UI prvek pro zobrazení penìz
    [SerializeField]
    private TextMeshProUGUI moneyChangeText; // Text pro zobrazení zmìny penìz (+ nebo -)

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "moneyData.json");
        LoadMoney();
    }

    private void Start()
    {
        FindMoneyUI();
        UpdateMoneyUI();
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
        }
        else
        {
            // Pokud hráè nemá dostatek penìz, nastaví je na 0
            Debug.LogError("Not enough money! Setting money to zero.");
            currentMoney = 0;
        }
        ShowMoneyChangeEffect(-amount); // Zobrazit zmìnu penìz s negativním efektem
        UpdateMoneyUI();
        SaveMoney();
    }


    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("Money added: " + amount + ". Total: " + currentMoney);
        ShowMoneyChangeEffect(amount); // Zobrazit zmìnu penìz s pozitivním efektem
        UpdateMoneyUI();
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

    private void UpdateMoneyUI()
    {
        if (moneyText == null) FindMoneyUI();

        if (moneyText != null)
        {
            moneyText.text = "Money: " + currentMoney;
        }
    }

    private void FindMoneyUI()
    {
        GameObject moneyTextObject = GameObject.FindWithTag("MoneyText");
        if (moneyTextObject != null)
        {
            moneyText = moneyTextObject.GetComponent<TextMeshProUGUI>();
        }

        GameObject moneyChangeTextObject = GameObject.FindWithTag("MoneyChangeText");
        if (moneyChangeTextObject != null)
        {
            moneyChangeText = moneyChangeTextObject.GetComponent<TextMeshProUGUI>();
        }
    }


    private void ShowMoneyChangeEffect(int amount)
    {
        if (moneyChangeText != null)
        {
            // Nastav text na zmìnu penìz
            moneyChangeText.text = (amount > 0 ? "+" : "") + amount.ToString() + "$";

            // Nastav barvu textu
            moneyChangeText.color = (amount > 0) ? Color.green : Color.red;

            // Ukaž efekt
            moneyChangeText.gameObject.SetActive(true);

            // Skryj efekt po 1 sekundì
            Invoke("HideMoneyChangeEffect", 1f);
        }
    }

    private void HideMoneyChangeEffect()
    {
        if (moneyChangeText != null)
        {
            moneyChangeText.gameObject.SetActive(false);
        }
    }

    [System.Serializable]
    private class MoneyData
    {
        public int money;
        public MoneyData(int money) { this.money = money; }
    }
}
