using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CinemachineTarget : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Najdi hráèe v nové scénì
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && virtualCamera != null)
        {
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;
        }
    }
}
