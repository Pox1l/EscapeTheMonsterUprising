using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefabs;  // Pole rùzných NPC prefabù
    public Transform player;         // Reference na hráèe
    public Tilemap grassTilemap;     // Tilemap, kde se spawnují NPC
    public int minNPCCount = 3;      // Minimální poèet NPC
    public int maxNPCCount = 8;      // Maximální poèet NPC

    private void Start()
    {
        int npcCount = Random.Range(minNPCCount, maxNPCCount + 1); // Náhodný poèet NPC
        SpawnNPCs(npcCount);
    }

    private void SpawnNPCs(int npcCount)
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
                Vector3 spawnPosition = grassTilemap.CellToWorld(randomCell) + new Vector3(0.5f, 0.5f, 0);

                // Náhodný výbìr NPC prefab
                GameObject randomNPCPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

                // Vytvoøení NPC
                GameObject npc = Instantiate(randomNPCPrefab, spawnPosition, Quaternion.identity);

                // Pøiøazení reference na hráèe
                NPCFollow followerScript = npc.GetComponent<NPCFollow>();
                if (followerScript != null)
                {
                    followerScript.player = player;
                }

                spawnedCount++;
            }
        }
    }
}
