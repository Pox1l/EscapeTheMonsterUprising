using UnityEngine;
using Cinemachine;

public class MapSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera miniMapCam;
    public CinemachineVirtualCamera worldMapCam;
    public GameObject worldMapPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isWorldMapActive = worldMapPanel.activeSelf;
            worldMapPanel.SetActive(!isWorldMapActive);

            if (isWorldMapActive)
            {
                miniMapCam.Priority = 10;  // Mini-mapa zpìt
                worldMapCam.Priority = 1;
            }
            else
            {
                miniMapCam.Priority = 1;
                worldMapCam.Priority = 10; // Pøepnutí na svìtovou mapu
            }
        }
    }
}
