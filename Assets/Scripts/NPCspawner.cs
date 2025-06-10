using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public Tilemap grassTilemap;
    public int minNPCCount = 3;
    public int maxNPCCount = 8;

    [Header("References")]
    public Transform player;

    [SerializeField]
    private GameObject[] npcPrefabs;
    private int totalNPC;
    private TextMeshProUGUI counter;

    /* ---------- UNITY ---------- */
    private void Awake()
    {
        // načtení všech NPC prefabů ze složky Resources/NPCs
        npcPrefabs = Resources.LoadAll<GameObject>("NPCs");
        Debug.Log($"Načteno {npcPrefabs.Length} prefabů z Resources/NPCs");


        if (npcPrefabs == null || npcPrefabs.Length == 0)
            Debug.LogWarning("❗ Žádné NPC prefaby nebyly nalezeny ve složce Resources/NPCs!");

        // najdi UI text podle tagu (jen jednou)
        if (GameObject.FindGameObjectWithTag("NPCCountText") is { } uiObj)
            counter = uiObj.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        int randomCount = Random.Range(minNPCCount, maxNPCCount + 1);
        SpawnNPCs(randomCount);

        foreach (NPCFollow npc in FindObjectsOfType<NPCFollow>())
            if (npc.Spawner == null) Register(npc);

        UpdateCounter();
    }

    /* ---------- REGISTRACE A ODEBÍRÁNÍ ---------- */
    public void Register(NPCFollow npc)
    {
        if (npc.Spawner != null) return;

        npc.Spawner = this;
        npc.onRescued += HandleNPCRescued;
        npc.onRemoved += HandleNPCRemoved;
        totalNPC++;
        UpdateCounter();
    }

    private void HandleNPCRescued()
    {
        totalNPC--;
        UpdateCounter();
    }

    private void HandleNPCRemoved()
    {
        totalNPC--;
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        if (counter) counter.text = $"NPC remains: {totalNPC}";
    }

    /* ---------- SPAWN ---------- */
    private void SpawnNPCs(int npcCount)
    {
        if (npcPrefabs == null || npcPrefabs.Length == 0) return;

        BoundsInt bounds = grassTilemap.cellBounds;
        int spawned = 0;

        while (spawned < npcCount)
        {
            Vector3Int cell = new(
                Random.Range(bounds.xMin, bounds.xMax),
                Random.Range(bounds.yMin, bounds.yMax), 0);

            if (!grassTilemap.HasTile(cell)) continue;

            Vector3 pos = grassTilemap.CellToWorld(cell) + new Vector3(.5f, .5f, 0);
            GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);

            if (go.TryGetComponent(out NPCFollow follow))
            {
                follow.player = player;
                Register(follow);
            }
            spawned++;
        }
    }
}
