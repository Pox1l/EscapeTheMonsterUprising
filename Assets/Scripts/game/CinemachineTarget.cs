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
        StartCoroutine(AssignCameraTarget());
    }

    private System.Collections.IEnumerator AssignCameraTarget()
    {
        GameObject player = null;

        // Poèkej, dokud se hráè ve scénì neobjeví
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            yield return null; // poèkej jeden frame
        }

        if (virtualCamera != null)
        {
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;
        }
    }

}
