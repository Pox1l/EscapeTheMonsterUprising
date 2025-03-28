using UnityEngine;
using Cinemachine;

public class CameraConfinerFix : MonoBehaviour
{
    private CinemachineConfiner2D confiner;

    void Start()
    {
        confiner = GetComponent<CinemachineConfiner2D>();

        if (confiner != null)
        {
            confiner.InvalidateCache(); // Znovu naète boundary collider
        }
    }
}
