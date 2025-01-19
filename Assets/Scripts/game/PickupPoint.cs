using UnityEngine;

public class HatchManager : MonoBehaviour
{
    public int rescuedNPCCount = 0; // Po�et zachr�n�n�ch NPC

    // Tato metoda se vol�, kdy� je NPC zachr�n�no
    public void AddRescuedNPC()
    {
        rescuedNPCCount++; // Zv��� po�et zachr�n�n�ch NPC
        Debug.Log($"NPC zachr�n�no! Celkem zachr�n�n�ch NPC: {rescuedNPCCount}");
    }
}
