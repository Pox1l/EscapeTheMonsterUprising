using UnityEngine;

public class RescuableNPC : MonoBehaviour
{
    private HatchManager hatchManager; // Odkaz na HatchManager
    private NPCFollow npcFollow; // Reference to NPCFollow component
    private bool isFollowing = false;

    private void Start()
    {
        // Getting reference to the NPCFollow component attached to the same GameObject
        npcFollow = GetComponent<NPCFollow>();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        // Debug v�pis pro sledov�n�, zda NPC vstupuje do bunkru
        if (collider2D.CompareTag("Hatch") && isFollowing)
        {
            Debug.Log("NPC entered Hatch area, attempting rescue.");
            RescueNPC();
        }
        else
        {
            Debug.Log($"NPC did not enter Hatch or is not following. isFollowing: {isFollowing}");
        }
    }

    private void RescueNPC()
    {
        Debug.Log($"NPC {gameObject.name} zachr�n�no!");
        isFollowing = false; // P�estane sledovat hr��e
        NPCFollow.followingNPCCount--; // Sn�en� po�tu sleduj�c�ch NPC

        // Aktualizace po�tu zachr�n�n�ch NPC v GameManageru
        GameManager.Instance.IncrementRescuedNPCCount(); // Update the total rescued NPCs in the GameManager

        // Pokud je k dispozici HatchManager, zavolej jeho metodu
        hatchManager = FindObjectOfType<HatchManager>();
        if (hatchManager != null)
        {
            hatchManager.AddRescuedNPC();
        }
        else
        {
            Debug.LogWarning("HatchManager not found in the scene.");
        }

        Destroy(gameObject); // NPC zmiz�
    }

    public void StartFollowing()
    {
        isFollowing = true;
        Debug.Log("NPC started following.");
    }

    public void StopFollowing()
    {
        isFollowing = false;
        Debug.Log("NPC stopped following.");
    }
}
