using UnityEngine;

public class KeyUIController : MonoBehaviour
{
    public GameObject keyIcon;

    void Update()
    {
        if (PlayerInventory.instance != null)
        {
            // Aktivuj nebo deaktivuj klíè podle stavu
            keyIcon.SetActive(PlayerInventory.instance.hasKey);
        }
    }
}
