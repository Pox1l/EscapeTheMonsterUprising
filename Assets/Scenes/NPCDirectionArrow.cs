using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class NPCDirectionArrow : MonoBehaviour
{
    public string playerTag = "Player";
    public string arrowTag = "DirectionArrow";
    public string npcTag = "NPC";
    public float hideDistance = 3f;

    private Transform player;
    private RectTransform arrowUI;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindReferences();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindReferences();
    }

    void FindReferences()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;

        GameObject arrowObj = GameObject.FindGameObjectWithTag(arrowTag);
        if (arrowObj != null)
            arrowUI = arrowObj.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (player == null || arrowUI == null)
        {
            FindReferences();
            return;
        }

        GameObject[] npcs = GameObject.FindGameObjectsWithTag(npcTag);
        if (npcs.Length == 0)
        {
            arrowUI.gameObject.SetActive(false);
            return;
        }

        GameObject closest = npcs
            .OrderBy(npc => Vector2.Distance(player.position, npc.transform.position))
            .First();

        float distance = Vector2.Distance(player.position, closest.transform.position);

        if (distance < hideDistance)
        {
            arrowUI.gameObject.SetActive(false);
            return;
        }

        arrowUI.gameObject.SetActive(true);

        Vector2 direction = (closest.transform.position - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowUI.rotation = Quaternion.Euler(0, 0, angle - 90); // -90 pokud šipka míøí výchozím smìrem nahoru
    }
}
