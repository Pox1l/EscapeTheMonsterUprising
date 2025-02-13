using UnityEngine;
using TMPro; // Importuj TextMeshPro namespace

public class CommandLine : MonoBehaviour
{
    public GameObject commandLineUI; // UI pro command line
    public TMP_InputField inputField; // TextMeshPro InputField pro zadání pøíkazu

    private bool isCommandLineActive = false;
    private static CommandLine instance; // Singleton instance

    void Awake()
    {
        // Pokud instance není nastavena, nastavíme ji a zachováme objekt mezi scénami
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Zachová objekt mezi scénami
        }
        else
        {
            Destroy(gameObject); // Znièí duplikáty objektu
        }
    }

    void Update()
    {
        // Tlaèítko pro zapnutí/vypnutí command line (stisk klávesy P)
        if (Input.GetKeyDown(KeyCode.P)) // Zmìnìno na P
        {
            ToggleCommandLine(); // Zapne nebo vypne command line
        }

        // Pokud je command line aktivní a stiskneš Enter, vykoná pøíkaz
        if (isCommandLineActive && Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCommand(inputField.text); // Vykoná pøíkaz bez zavøení command line
        }
    }

    // Funkce pro zapnutí/vypnutí command line
    void ToggleCommandLine()
    {
        isCommandLineActive = !isCommandLineActive; // Pøepnutí stavu
        commandLineUI.SetActive(isCommandLineActive); // Zobrazí nebo skryje command line

        Debug.Log("Command line active: " + isCommandLineActive); // Debugging

        if (isCommandLineActive)
        {
            inputField.ActivateInputField(); // Aktivuje InputField pro zadávání
            inputField.text = ""; // Vymaže text pøi otevøení command line
        }
        else
        {
            inputField.DeactivateInputField(); // Deaktivuje InputField
        }
    }

    // Funkce pro vykonání pøíkazu
    void ExecuteCommand(string command)
    {
        if (command == "clear")
        {
            inputField.text = ""; // Vymaže text
            Debug.Log("Command cleared"); // Vypíše v logu, že byla vymazána
        }
        else if (command == "heal")
        {
            // Zavolá metodu Heal na instanci PlayerHealth pro uzdravení hráèe
            PlayerHealth.Instance.Heal(10); // Uzdraví hráèe o 10 bodù (mùžeš upravit hodnotu)
            Debug.Log("Player healed!"); // Vypíše v logu, že postava byla uzdravena
        }
        else
        {
            Debug.Log("Unknown command: " + command); // Vypíše v logu neznámý pøíkaz
        }

        // Command line zùstane otevøená, neprovádí se Toggle
    }
}
