using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManagement pro naèítání scén

public class MenuController : MonoBehaviour
{
    public GameObject settingsPanel; // Odkaz na panel nastavení
    public GameObject mainMenuPanel; // Odkaz na hlavní menu panel

    // Funkce pro naètení scény "Lobby"
    public void StartLobby()
    {
        SceneManager.LoadScene(1); // Naète scénu s indexem 1
    }

    // Funkce pro ukonèení hry
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

   
    
}
