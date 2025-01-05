using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // Prefab monstera
    public float spawnDistance = 10f; // Vzdálenost od okrajù kamery pro spawn
    public float spawnInterval = 3f; // Interval mezi spawnováním monster
    public int maxMonsters = 10;     // Maximální poèet monster ve scénì
    public Tilemap grassTilemap;     // Tilemap, kde se monstra mohou spawnovat

    private Camera mainCamera;
    private float nextSpawnTime;

    void Start()
    {
        mainCamera = Camera.main;
        nextSpawnTime = Time.time + spawnInterval;

        if (grassTilemap == null)
        {
            Debug.LogError("Tilemap pro trávu není pøiøazená! Monstra se nebudou spawnovat správnì.");
        }
    }

    void Update()
    {
        // Spawnuj monstra, pokud je to èasovì vhodné a nepøekroèen limit
        if (Time.time >= nextSpawnTime && CountMonsters() < maxMonsters)
        {
            SpawnMonster();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnMonster()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Hlavní kamera není nastavena!");
            return;
        }

        // Výpoèet hranic kamery
        Vector3 cameraPosition = mainCamera.transform.position;
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = mainCamera.aspect * halfHeight;

        // Náhodný výbìr strany, kde se monstrum spawnuje (0 = nahoøe, 1 = dole, 2 = vlevo, 3 = vpravo)
        int spawnSide = Random.Range(0, 4);

        Vector3 spawnPosition = Vector3.zero;

        switch (spawnSide)
        {
            case 0: // Nahoøe
                spawnPosition = new Vector3(
                    Random.Range(cameraPosition.x - halfWidth, cameraPosition.x + halfWidth),
                    cameraPosition.y + halfHeight + spawnDistance,
                    0);
                break;

            case 1: // Dole
                spawnPosition = new Vector3(
                    Random.Range(cameraPosition.x - halfWidth, cameraPosition.x + halfWidth),
                    cameraPosition.y - halfHeight - spawnDistance,
                    0);
                break;

            case 2: // Vlevo
                spawnPosition = new Vector3(
                    cameraPosition.x - halfWidth - spawnDistance,
                    Random.Range(cameraPosition.y - halfHeight, cameraPosition.y + halfHeight),
                    0);
                break;

            case 3: // Vpravo
                spawnPosition = new Vector3(
                    cameraPosition.x + halfWidth + spawnDistance,
                    Random.Range(cameraPosition.y - halfHeight, cameraPosition.y + halfHeight),
                    0);
                break;
        }

        // Pøevod na pozici v Tilemap
        Vector3Int tilePosition = grassTilemap.WorldToCell(spawnPosition);

        // Kontrola, zda je na Tilemapì tráva
        if (grassTilemap.HasTile(tilePosition))
        {
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log($"Pozice {spawnPosition} není na Tilemapì trávy. Monstrum se nespawnovalo.");
        }
    }

    int CountMonsters()
    {
        // Spoèítá aktuální poèet monster ve scénì podle tagu
        return GameObject.FindGameObjectsWithTag("Monster").Length;
    }
}
