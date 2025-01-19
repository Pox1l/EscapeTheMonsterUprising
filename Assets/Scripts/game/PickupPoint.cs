using UnityEngine;

public class HatchManager : MonoBehaviour
{
    public int rescuedNPCCount = 0; // Poèet zachránìných NPC

    // Tato metoda se volá, když je NPC zachránìno
    public void AddRescuedNPC()
    {
        rescuedNPCCount++; // Zvýší poèet zachránìných NPC
        Debug.Log($"NPC zachránìno! Celkem zachránìných NPC: {rescuedNPCCount}");
    }
}
