using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;  // Prefab NPC
    public Transform player;      // Reference na hráèe
    public Tilemap grassTilemap;  // Tilemap, kde se spawnují NPC
    public int npcCount = 5;      // Poèet NPC k vytvoøení

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
                // Pøevod buòky na svìtové souøadnice
                Vector3 spawnPosition = grassTilemap.CellToWorld(randomCell) + new Vector3(0.5f, 0.5f, 0);

                // Vytvoøení NPC
                GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);

                // Pøiøazení reference na hráèe
                NPCFollow followerScript = npc.GetComponent<NPCFollow>();
                if (followerScript != null)
                {
                    followerScript.player = player; // Pøiøazení hráèe
                }

                spawnedCount++;
            }
        }
    }
}
