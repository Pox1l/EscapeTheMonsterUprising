using UnityEngine;

public class XPDisplayTab : MonoBehaviour
{
    public GameObject xpTabPanel; // Nastav v Inspectoru svùj panel s XP UI

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isActive = xpTabPanel.activeSelf;
            xpTabPanel.SetActive(!isActive);

            if (!isActive) // Pokud právì otevíráme TAB
            {
                Player_XP.Instance.FindXPUI();
                Player_XP.Instance.UpdateXPUI();
            }
        }
    }
}
