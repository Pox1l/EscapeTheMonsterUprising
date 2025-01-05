using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // Prefab monstera
    public float spawnDistance = 10f; // Vzd�lenost od okraj� kamery pro spawn
    public float spawnInterval = 3f; // Interval mezi spawnov�n�m monster
    public int maxMonsters = 10;     // Maxim�ln� po�et monster ve sc�n�
    public Tilemap grassTilemap;     // Tilemap, kde se monstra mohou spawnovat

    private Camera mainCamera;
    private float nextSpawnTime;

    void Start()
    {
        mainCamera = Camera.main;
        nextSpawnTime = Time.time + spawnInterval;

        if (grassTilemap == null)
        {
            Debug.LogError("Tilemap pro tr�vu nen� p�i�azen�! Monstra se nebudou spawnovat spr�vn�.");
        }
    }

    void Update()
    {
        // Spawnuj monstra, pokud je to �asov� vhodn� a nep�ekro�en limit
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
            Debug.LogError("Hlavn� kamera nen� nastavena!");
            return;
        }

        // V�po�et hranic kamery
        Vector3 cameraPosition = mainCamera.transform.position;
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = mainCamera.aspect * halfHeight;

        // N�hodn� v�b�r strany, kde se monstrum spawnuje (0 = naho�e, 1 = dole, 2 = vlevo, 3 = vpravo)
        int spawnSide = Random.Range(0, 4);

        Vector3 spawnPosition = Vector3.zero;

        switch (spawnSide)
        {
            case 0: // Naho�e
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

        // P�evod na pozici v Tilemap
        Vector3Int tilePosition = grassTilemap.WorldToCell(spawnPosition);

        // Kontrola, zda je na Tilemap� tr�va
        if (grassTilemap.HasTile(tilePosition))
        {
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log($"Pozice {spawnPosition} nen� na Tilemap� tr�vy. Monstrum se nespawnovalo.");
        }
    }

    int CountMonsters()
    {
        // Spo��t� aktu�ln� po�et monster ve sc�n� podle tagu
        return GameObject.FindGameObjectsWithTag("Monster").Length;
    }
}
