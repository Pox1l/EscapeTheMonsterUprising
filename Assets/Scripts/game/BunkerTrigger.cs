using UnityEngine;
using UnityEngine.SceneManagement;

public class BunkerTrigger : MonoBehaviour
{
    public GameObject interactText;
    public float holdTime = 2f;
    private float holdProgress = 0f;
    private bool playerInRange = false;

    public int targetSceneIndex = 1;

    private bool triggered = false;

    void Start()
    {
        if (interactText != null)
            interactText.SetActive(false);
    }

    void Update()
    {
        if (triggered) return;

        if (playerInRange && Input.GetKey(KeyCode.E))
        {
            holdProgress += Time.deltaTime;
            if (holdProgress >= holdTime)
            {
                triggered = true;
                MovePlayerToScene();
            }
        }
        else if (playerInRange)
        {
            holdProgress = Mathf.Max(0, holdProgress - Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactText != null)
                interactText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            holdProgress = 0f;
            if (interactText != null)
                interactText.SetActive(false);
        }
    }

    private void MovePlayerToScene()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        DontDestroyOnLoad(player);

        // Pøehrát zvuk otevøení dveøí
        if (AudioManager.instance != null)
            AudioManager.instance.OpenDoor();

        // Pøechod pomocí SceneLoaderu
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if (loader != null)
        {
            loader.LoadSceneWithTransition(targetSceneIndex, () =>
            {
                GameObject entryPoint = GameObject.FindGameObjectWithTag("EntryPoint");
                if (entryPoint != null)
                    player.transform.position = entryPoint.transform.position;
            });
        }
        else
        {
            // Záložní pøechod bez loaderu
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (scene.buildIndex == targetSceneIndex)
                {
                    GameObject entryPoint = GameObject.FindGameObjectWithTag("EntryPoint");
                    if (entryPoint != null)
                        player.transform.position = entryPoint.transform.position;
                }
            };
            SceneManager.LoadScene(targetSceneIndex);
        }
    }
}
