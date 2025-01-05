using UnityEngine;
using UnityEngine.SceneManagement;  // Import SceneManagement pro naèítání scén

public class MenuController : MonoBehaviour
{
    // Funkce pro naètení scény "Lobby"
    public void StartLobby()
    {
        // Naète scénu s názvem "Lobby"
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
