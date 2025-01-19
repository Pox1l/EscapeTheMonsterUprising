using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;  // Prefab NPC
    public Transform player;      // Reference na hr��e
    public Tilemap grassTilemap;  // Tilemap, kde se spawnuj� NPC
    public int npcCount = 5;      // Po�et NPC k vytvo�en�

    private void Start()
    {
        SpawnNPCs();
    }

    private void SpawnNPCs()
    {
        BoundsInt bounds = grassTilemap.cellBounds;
        int spawnedCount = 0;

        while (spawnedCount < npcCount)
        {
            Vector3Int randomCell = new Vector3Int(
                Random.Range(bounds.xMin, bounds.xMax),
                Random.Range(bounds.yMin, bounds.yMax),
                0
            );

            if (grassTilemap.HasTile(randomCell))
            {
                // P�evod bu�ky na sv�tov� sou�adnice
                Vector3 spawnPosition = grassTilemap.CellToWorld(randomCell) + new Vector3(0.5f, 0.5f, 0);

                // Vytvo�en� NPC
                GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);

                // P�i�azen� reference na hr��e
                NPCFollow followerScript = npc.GetComponent<NPCFollow>();
                if (followerScript != null)
                {
                    followerScript.player = player; // P�i�azen� hr��e
                }

                spawnedCount++;
            }
        }
    }
}
