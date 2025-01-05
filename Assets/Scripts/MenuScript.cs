using UnityEngine;
using UnityEngine.SceneManagement;  // Import SceneManagement pro na��t�n� sc�n

public class MenuController : MonoBehaviour
{
    // Funkce pro na�ten� sc�ny "Lobby"
    public void StartLobby()
    {
        // Na�te sc�nu s n�zvem "Lobby"
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
