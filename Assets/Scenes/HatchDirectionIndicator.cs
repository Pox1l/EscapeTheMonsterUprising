using UnityEngine;
using UnityEngine.SceneManagement;

public class HatchDirectionIndicator : MonoBehaviour
{
    public string playerTag = "Player";
    public string arrowTag = "DirectionArrow";
    public string hatchTag = "Hatch";
    public float hideDistance = 2f;

    private Transform player;
    private Transform hatch;
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

        GameObject hatchObj = GameObject.FindGameObjectWithTag(hatchTag);
        if (hatchObj != null)
            hatch = hatchObj.transform;
    }

    void Update()
    {
        if (player == null || arrowUI == null || hatch == null)
        {
            FindReferences();
            return;
        }

        float distance = Vector2.Distance(player.position, hatch.position);

        if (distance < hideDistance)
        {
            arrowUI.gameObject.SetActive(false);
            return;
        }

        arrowUI.gameObject.SetActive(true);

        Vector2 direction = (hatch.position - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowUI.rotation = Quaternion.Euler(0, 0, angle - 90); // -90 pokud šipka míøí výchozím smìrem nahoru
    }
}
