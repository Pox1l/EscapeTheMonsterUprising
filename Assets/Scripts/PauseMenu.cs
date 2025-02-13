using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public string tagToDestroyInMenu = "Player";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        DestroyObjectByTag(tagToDestroyInMenu);
        SceneManager.LoadScene("Menu");
    }
    public void Quit() 
    {
        Debug.Log("Quiting...");
        Application.Quit();
    }


    private void DestroyObjectByTag(string tag)
    {
        GameObject objectToDestroy = GameObject.FindGameObjectWithTag(tag);
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
            Debug.Log($"Destroyed object with tag {tag}");
        }
        else
        {
            Debug.Log($"No object found with tag {tag}.");
        }
    }
}
