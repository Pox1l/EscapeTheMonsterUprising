using UnityEngine;
using System.Collections;

public class ShadowSpawner : MonoBehaviour
{
    public GameObject[] shadowPrefabs;
    private Transform player;
    public float spawnRadius = 5f;
    public float spawnInterval = 2f;

    void Start()
    {
        StartCoroutine(FindPlayerAfterDelay());
        InvokeRepeating(nameof(SpawnShadow), 0f, spawnInterval);
    }

    IEnumerator FindPlayerAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // Poèká 0.5 sekundy, aby se hráè mohl naèíst

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            Debug.Log("Hráè nalezen: " + player.name);
        }
        else
        {
            Debug.LogWarning("Hráè nebyl nalezen! Ujisti se, že má správný Tag 'Player'.");
        }
    }

    void SpawnShadow()
    {
        if (shadowPrefabs.Length == 0 || player == null) return;

        GameObject randomShadowPrefab = shadowPrefabs[Random.Range(0, shadowPrefabs.Length)];
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(player.position.x + randomOffset.x, player.position.y + randomOffset.y, 0);

        GameObject newShadow = Instantiate(randomShadowPrefab, spawnPosition, Quaternion.identity);
        float scale = Random.Range(0.8f, 1.2f);
        newShadow.transform.localScale = new Vector3(scale, scale, 1);
        newShadow.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, Random.Range(0.3f, 0.6f));

        Destroy(newShadow, Random.Range(5f, 10f));
    }
}
