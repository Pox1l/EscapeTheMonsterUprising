using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance { get; private set; }

    [SerializeField] private int startingMoney = 100; // Poèáteèní peníze
    private int currentMoney;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Pøetrvá mezi scénami
        currentMoney = startingMoney;
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
            Debug.LogError("Not enough money!");
        }
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("Money added: " + amount + ". Total: " + currentMoney);
    }
}
