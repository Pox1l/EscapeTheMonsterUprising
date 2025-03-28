using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CameraConfinerUpdater : MonoBehaviour
{
    private CinemachineConfiner2D confiner;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        confiner = GetComponent<CinemachineConfiner2D>();

        if (confiner != null)
        {
            confiner.m_BoundingShape2D = FindObjectOfType<PolygonCollider2D>();
            confiner.InvalidateCache();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
