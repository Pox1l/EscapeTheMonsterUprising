using UnityEngine;
using TMPro; // Importuj TextMeshPro namespace

public class CommandLine : MonoBehaviour
{
    public GameObject commandLineUI; // UI pro command line
    public TMP_InputField inputField; // TextMeshPro InputField pro zad�n� p��kazu

    private bool isCommandLineActive = false;
    private static CommandLine instance; // Singleton instance

    void Awake()
    {
        // Pokud instance nen� nastavena, nastav�me ji a zachov�me objekt mezi sc�nami
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Zachov� objekt mezi sc�nami
        }
        else
        {
            Destroy(gameObject); // Zni�� duplik�ty objektu
        }
    }

    void Update()
    {
        // Tla��tko pro zapnut�/vypnut� command line (stisk kl�vesy P)
        if (Input.GetKeyDown(KeyCode.P)) // Zm�n�no na P
        {
            ToggleCommandLine(); // Zapne nebo vypne command line
        }

        // Pokud je command line aktivn� a stiskne� Enter, vykon� p��kaz
        if (isCommandLineActive && Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCommand(inputField.text); // Vykon� p��kaz bez zav�en� command line
        }
    }

    // Funkce pro zapnut�/vypnut� command line
    void ToggleCommandLine()
    {
        isCommandLineActive = !isCommandLineActive; // P�epnut� stavu
        commandLineUI.SetActive(isCommandLineActive); // Zobraz� nebo skryje command line

        Debug.Log("Command line active: " + isCommandLineActive); // Debugging

        if (isCommandLineActive)
        {
            inputField.ActivateInputField(); // Aktivuje InputField pro zad�v�n�
            inputField.text = ""; // Vyma�e text p�i otev�en� command line
        }
        else
        {
            inputField.DeactivateInputField(); // Deaktivuje InputField
        }
    }

    // Funkce pro vykon�n� p��kazu
    void ExecuteCommand(string command)
    {
        if (command == "clear")
        {
            inputField.text = ""; // Vyma�e text
            Debug.Log("Command cleared"); // Vyp�e v logu, �e byla vymaz�na
        }
        else if (command == "heal")
        {
            // Zavol� metodu Heal na instanci PlayerHealth pro uzdraven� hr��e
            PlayerHealth.Instance.Heal(10); // Uzdrav� hr��e o 10 bod� (m��e� upravit hodnotu)
            Debug.Log("Player healed!"); // Vyp�e v logu, �e postava byla uzdravena
        }
        else
        {
            Debug.Log("Unknown command: " + command); // Vyp�e v logu nezn�m� p��kaz
        }

        // Command line z�stane otev�en�, neprov�d� se Toggle
    }
}
