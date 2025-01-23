using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManagement pro na��t�n� sc�n

public class MenuController : MonoBehaviour
{
    public GameObject settingsPanel; // Odkaz na panel nastaven�
    public GameObject mainMenuPanel; // Odkaz na hlavn� menu panel

    // Funkce pro na�ten� sc�ny "Lobby"
    public void StartLobby()
    {
        SceneManager.LoadScene(1); // Na�te sc�nu s indexem 1
    }

    // Funkce pro ukon�en� hry
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

   
    
}
