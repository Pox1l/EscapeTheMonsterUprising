using UnityEngine;
using TMPro;
using System.Collections;

public class CommandLine : MonoBehaviour
{
    public GameObject commandLineUI; // UI pro command line
    public TMP_InputField inputField; // TextMeshPro InputField pro zadání pøíkazu
    public TMP_Text feedbackText; // Text pro zobrazení zprávy

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
        if (Input.GetKeyDown(KeyCode.F2))
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
                    ShowFeedback("Player healed by 10 HP!");
                    Debug.Log("Player healed by 10 HP!");
                }
                else
                {
                    ShowFeedback("PlayerHealth instance not found!");
                    Debug.LogError("PlayerHealth instance not found!");
                }
                break;

            case "money":
                if (PlayerMoney.Instance != null)
                {
                    PlayerMoney.Instance.AddMoney(50);
                    ShowFeedback("Added 50 money!");
                    Debug.Log("Added 50 money!");
                }
                else
                {
                    ShowFeedback("PlayerMoney instance not found!");
                    Debug.LogError("PlayerMoney instance not found!");
                }
                break;

            case "dmg":
                if (PlayerHealth.Instance != null)
                {
                    PlayerHealth.Instance.TakeDamage(10);
                    ShowFeedback("Player took 10 damage!");
                    Debug.Log("Player took 10 damage!");
                }
                else
                {
                    ShowFeedback("PlayerHealth instance not found!");
                    Debug.LogError("PlayerHealth instance not found!");
                }
                break;

            case "npc":
                SaveSystem.AddRescuedNPCs(1);
                ShowFeedback("Increased rescued NPCs by 1!");
                Debug.Log("Increased rescued NPCs by 1!");
                break;

            default:
                ShowFeedback("Unknown command: " + command);
                Debug.Log("Unknown command: " + command);
                break;
        }
    }

    void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            StopAllCoroutines(); // Zastaví pøedchozí èekání
            StartCoroutine(ClearFeedbackAfterSeconds(3f)); // Skryje za 3 sekundy
        }
    }

    IEnumerator ClearFeedbackAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (feedbackText != null)
            feedbackText.text = "";
    }
}
