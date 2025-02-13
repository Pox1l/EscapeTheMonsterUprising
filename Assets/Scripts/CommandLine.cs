using UnityEngine;
using TMPro; // Importuje TextMeshPro namespace

public class CommandLine : MonoBehaviour
{
    public GameObject commandLineUI; // UI pro command line
    public TMP_InputField inputField; // TextMeshPro InputField pro zadání pøíkazu

    private bool isCommandLineActive = false;
    private static CommandLine instance; // Singleton instance

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleCommandLine();
        }

        if (isCommandLineActive && Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCommand(inputField.text.Trim()); // Odebere mezery kolem textu
        }
    }

    void ToggleCommandLine()
    {
        isCommandLineActive = !isCommandLineActive;
        commandLineUI.SetActive(isCommandLineActive);

        if (isCommandLineActive)
        {
            inputField.ActivateInputField();
            inputField.text = "";
        }
        else
        {
            inputField.DeactivateInputField();
        }
    }

    void ExecuteCommand(string command)
    {
        if (string.IsNullOrEmpty(command)) return;

        switch (command.ToLower())
        {
            case "heal":
                if (PlayerHealth.Instance != null)
                {
                    PlayerHealth.Instance.Heal(10);
                    Debug.Log("Player healed by 10 HP!");
                }
                else
                {
                    Debug.LogError("PlayerHealth instance not found!");
                }
                break;

            case "money":
                if (PlayerMoney.Instance != null)
                {
                    PlayerMoney.Instance.AddMoney(50);
                    Debug.Log("Added 50 money!");
                }
                else
                {
                    Debug.LogError("PlayerMoney instance not found!");
                }
                break;

            case "dmg":
                if (PlayerHealth.Instance != null)
                {
                    PlayerHealth.Instance.TakeDamage(10);
                    Debug.Log("Player took 10 damage!");
                }
                else
                {
                    Debug.LogError("PlayerHealth instance not found!");
                }
                break;

            default:
                Debug.Log("Unknown command: " + command);
                break;
        }
    }
}
