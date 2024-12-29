using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // The monster prefab to spawn
    public Transform player;         // Reference to the player
    public float spawnDistance = 10f; // Distance outside the camera bounds to spawn monsters
    public float spawnInterval = 3f; // Time interval between spawns
    public int maxMonsters = 10; // Maximum number of monsters allowed at a time

    private Camera mainCamera;
    private float nextSpawnTime;

    void Start()
    {
        mainCamera = Camera.main;
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        // Check if it's time to spawn a new monster
        if (Time.time >= nextSpawnTime && CountMonsters() < maxMonsters)
        {
            SpawnMonster();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnMonster()
    {
        // Get the camera bounds in world space
        Vector3 cameraPosition = mainCamera.transform.position;
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = mainCamera.aspect * halfHeight;

        // Randomize the side where the monster will spawn (0 = top, 1 = bottom, 2 = left, 3 = right)
        int spawnSide = Random.Range(0, 4);

        Vector3 spawnPosition = Vector3.zero;

        switch (spawnSide)
        {
            case 0: // Top
                spawnPosition = new Vector3(Random.Range(cameraPosition.x - halfWidth, cameraPosition.x + halfWidth), cameraPosition.y + halfHeight + spawnDistance, 0);
                break;
            case 1: // Bottom
                spawnPosition = new Vector3(Random.Range(cameraPosition.x - halfWidth, cameraPosition.x + halfWidth), cameraPosition.y - halfHeight - spawnDistance, 0);
                break;
            case 2: // Left
                spawnPosition = new Vector3(cameraPosition.x - halfWidth - spawnDistance, Random.Range(cameraPosition.y - halfHeight, cameraPosition.y + halfHeight), 0);
                break;
            case 3: // Right
                spawnPosition = new Vector3(cameraPosition.x + halfWidth + spawnDistance, Random.Range(cameraPosition.y - halfHeight, cameraPosition.y + halfHeight), 0);
                break;
        }

        // Instantiate the monster at the calculated position
        GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

        // Assign the player reference to the monster's script
        EnemyController followScript = monster.GetComponent<EnemyController>();
        if (followScript != null)
        {
            followScript.player = player;
        }
    }

    int CountMonsters()
    {
        // Count the current number of monsters in the scene
        return GameObject.FindGameObjectsWithTag("Monster").Length;
    }
}
