using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsterPrefabs; // Seznam rùzných prefabrikátù monster
    public float spawnDistance = 10f;       // Vzdálenost od okrajù kamery pro spawn
    public float spawnInterval = 3f;       // Interval mezi spawnováním monster
    public int maxMonsters = 10;           // Maximální poèet monster ve scénì
    public float increaseInterval = 30f;  // Interval pro zvyšování maximálního poètu monster
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
            Debug.LogError("Tilemap pro trávu není pøiøazená! Monstra se nebudou spawnovat správnì.");
        }

        if (monsterPrefabs == null || monsterPrefabs.Count == 0)
        {
            Debug.LogError("Seznam monster je prázdný! Pøidejte alespoò jedno monstrum do seznamu.");
        }
    }

    void Update()
    {
        // Zvyšování maximálního poètu monster
        if (Time.time >= nextIncreaseTime)
        {
            maxMonsters++;
            nextIncreaseTime = Time.time + increaseInterval;
            Debug.Log($"Maximální poèet monster zvýšen na {maxMonsters}.");
        }

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

        // Náhodný výbìr strany, kde se monstrum spawnuje
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
            // Náhodný výbìr typu monstra
            GameObject randomMonster = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
            Instantiate(randomMonster, spawnPosition, Quaternion.identity);
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
