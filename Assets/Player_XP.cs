using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class Player_XP : MonoBehaviour
{
    public static Player_XP Instance { get; private set; }

    [SerializeField] private int startingXP = 0;
    private int currentXP;
    private string savePath;

    public TextMeshProUGUI xpText;
    private TextMeshProUGUI xpChangeText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        savePath = Path.Combine(Application.persistentDataPath, "xpData.json");
        LoadXP();
    }

    private void Start()
    {
        FindXPUI();
        UpdateXPUI();
    }

    private void OnApplicationQuit()
    {
        SaveXP();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Zkus znovu najít UI a aktualizovat
        FindXPUI();
        UpdateXPUI();
    }

    public int GetXP()
    {
        return currentXP;
    }

    // Metoda, která zkontroluje, zda hráè má dost XP
    public bool HasEnoughXP(int amount)
    {
        return currentXP >= amount;
    }

    public void SpendXP(int amount)
    {
        if (HasEnoughXP(amount))
        {
            currentXP -= amount;
            Debug.Log("XP spent: " + amount + ". Remaining: " + currentXP);
        }
        else
        {
            Debug.LogError("Not enough XP! Setting XP to zero.");
            currentXP = 0;
        }
        ShowXPChange(-amount);
        UpdateXPUI();
        SaveXP();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log("XP added: " + amount + ". Total: " + currentXP);
        ShowXPChange(amount);
        UpdateXPUI();
        SaveXP();
    }

    private void SaveXP()
    {
        string json = JsonUtility.ToJson(new XPData(currentXP));
        File.WriteAllText(savePath, json);
        Debug.Log("XP saved to " + savePath);
    }

    private void LoadXP()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            XPData data = JsonUtility.FromJson<XPData>(json);
            currentXP = data.xp;
        }
        else
        {
            currentXP = startingXP;
        }
        Debug.Log("XP loaded: " + currentXP);
    }

    public void UpdateXPUI()
    {
        if (xpText == null)
            FindXPUI();

        if (xpText != null)
        {
            xpText.text = "Player XP: " + currentXP;
        }
    }

    public void FindXPUI()
    {
        GameObject xpTextObject = GameObject.FindWithTag("XPText");
        if (xpTextObject != null)
        {
            xpText = xpTextObject.GetComponent<TextMeshProUGUI>();
        }

        // Pokud chceš, mùžeš najít i xpChangeText obdobnì
        // GameObject xpChangeTextObject = GameObject.FindWithTag("XPChangeText");
        // if (xpChangeTextObject != null)
        // {
        //     xpChangeText = xpChangeTextObject.GetComponent<TextMeshProUGUI>();
        // }
    }

    private void ShowXPChange(int amount)
    {
        if (xpChangeText != null)
        {
            xpChangeText.text = (amount > 0 ? "+" : "") + amount.ToString() + " XP";
        }
    }

    [System.Serializable]
    private class XPData
    {
        public int xp;
        public XPData(int xp)
        {
            this.xp = xp;
        }
    }
}
