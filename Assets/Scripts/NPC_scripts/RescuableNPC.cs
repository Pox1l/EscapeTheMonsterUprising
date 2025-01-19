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
        // Debug výpis pro sledování, zda NPC vstupuje do bunkru
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
        Debug.Log($"NPC {gameObject.name} zachránìno!");
        isFollowing = false; // Pøestane sledovat hráèe
        NPCFollow.followingNPCCount--; // Snížení poètu sledujících NPC

        // Aktualizace poètu zachránìných NPC v GameManageru
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

        Destroy(gameObject); // NPC zmizí
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
