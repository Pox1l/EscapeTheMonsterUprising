using UnityEngine;
using UnityEngine.SceneManagement;

public class BunkerExit : MonoBehaviour
{
    public GameObject interactText;
    public Vector3 fallbackPosition = new Vector3(0, 0, 0);
    private bool playerInRange = false;

    void Start()
    {
        if (interactText != null)
            interactText.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                DontDestroyOnLoad(player);

            // Zvuk pøes AudioManager
            if (AudioManager.instance != null)
                AudioManager.instance.CloseDoor();

            SceneLoader loader = FindObjectOfType<SceneLoader>();
            if (loader != null)
            {
                loader.LoadSceneWithTransition(2, () =>
                {
                    GameObject entryPoint = GameObject.FindGameObjectWithTag("EntryPoint");
                    if (player != null && entryPoint != null)
                    {
                        player.transform.position = entryPoint.transform.position;
                    }
                    else if (player != null)
                    {
                        player.transform.position = fallbackPosition;
                        Debug.LogWarning("EntryPoint nebyl nalezen, hráè pøesunut na fallback pozici.");
                    }
                });
            }
            else
            {
                // Záložní pøechod bez loaderu
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(2);
            }
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
            if (interactText != null)
                interactText.SetActive(false);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject entryPoint = GameObject.FindGameObjectWithTag("EntryPoint");

        if (player != null && entryPoint != null)
        {
            player.transform.position = entryPoint.transform.position;
        }
        else if (player != null)
        {
            player.transform.position = fallbackPosition;
            Debug.LogWarning("EntryPoint nebyl nalezen, hráè pøesunut na fallback pozici.");
        }
    }
}
