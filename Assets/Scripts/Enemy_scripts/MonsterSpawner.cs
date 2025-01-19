using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsterPrefabs; // Seznam r�zn�ch prefabrik�t� monster
    public float spawnDistance = 10f;       // Vzd�lenost od okraj� kamery pro spawn
    public float spawnInterval = 3f;       // Interval mezi spawnov�n�m monster
    public int maxMonsters = 10;           // Maxim�ln� po�et monster ve sc�n�
    public float increaseInterval = 30f;  // Interval pro zvy�ov�n� maxim�ln�ho po�tu monster
    public Tilemap grassTilemap;           // Tilemap, kde se monstra mohou spawnovat

    private Camera mainCamera;
    private float nextSpawnTime;
    private float nextIncreaseTime;

    void Start()
    {
        mainCamera = Camera.main;
        nextSpawnTime = Time.time + spawnInterval;
        nextIncreaseTime = Time.time + increaseInterval;

        if (grassTilemap == null)
        {
            Debug.LogError("Tilemap pro tr�vu nen� p�i�azen�! Monstra se nebudou spawnovat spr�vn�.");
        }

        if (monsterPrefabs == null || monsterPrefabs.Count == 0)
        {
            Debug.LogError("Seznam monster je pr�zdn�! P�idejte alespo� jedno monstrum do seznamu.");
        }
    }

    void Update()
    {
        // Zvy�ov�n� maxim�ln�ho po�tu monster
        if (Time.time >= nextIncreaseTime)
        {
            maxMonsters++;
            nextIncreaseTime = Time.time + increaseInterval;
            Debug.Log($"Maxim�ln� po�et monster zv��en na {maxMonsters}.");
        }

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

        // N�hodn� v�b�r strany, kde se monstrum spawnuje
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
            // N�hodn� v�b�r typu monstra
            GameObject randomMonster = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
            Instantiate(randomMonster, spawnPosition, Quaternion.identity);
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
