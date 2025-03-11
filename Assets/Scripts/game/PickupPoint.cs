using UnityEngine;

public class HatchManager : MonoBehaviour
{
    public int rescuedNPCCount = 0; // Poèet zachránìných NPC

    private void Start()
    {
        // Naètení dat z JSON
        rescuedNPCCount = SaveSystem.LoadNPCCount();
        Debug.Log($"Naèteno {rescuedNPCCount} zachránìných NPC.");
    }

    public void AddRescuedNPC()
    {
        rescuedNPCCount++; // Zvýší poèet zachránìných NPC
        Debug.Log($"NPC zachránìno! Celkem zachránìných NPC: {rescuedNPCCount}");

        // Uložit nové èíslo do JSON
        SaveSystem.SaveNPCCount(rescuedNPCCount);
    }
}
