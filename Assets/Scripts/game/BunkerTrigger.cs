using UnityEngine;
using UnityEngine.UI; // Pro zobrazení UI textu
using UnityEngine.SceneManagement; // Pro pøepínání scén

public class BunkerTrigger : MonoBehaviour
{
    public GameObject interactText; // Text, který se zobrazí (napø. "Drž E pro vstup")
    public float holdTime = 2f; // Doba držení E pro pøechod
    private float holdProgress = 0f; // Sledování držení klávesy

    private bool playerInRange = false; // Sleduje, zda je hráè v oblasti

    // ID scény, kam se pøechází
    public int targetSceneIndex = 1; // Èíslo scény (napø. 1 = GameInside)

    void Start()
    {
        if (interactText != null)
            interactText.SetActive(false); // Schová text pøi startu
    }

    void Update()
    {
        if (playerInRange && Input.GetKey(KeyCode.E))
        {
            holdProgress += Time.deltaTime;

            if (holdProgress >= holdTime)
            {
                MovePlayerToScene();
            }
        }
        else if (playerInRange)
        {
            holdProgress = Mathf.Max(0, holdProgress - Time.deltaTime); // Resetuje postup, pokud E není drženo
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactText != null)
                interactText.SetActive(true); // Zobrazí text
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            holdProgress = 0f; // Resetuje postup držení
            if (interactText != null)
                interactText.SetActive(false); // Skryje text
        }
    }

    private void MovePlayerToScene()
    {
        // Najde hráèe
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            DontDestroyOnLoad(player); // Zajistí, že hráè nezmizí
            SceneManager.LoadScene(targetSceneIndex); // Pøepne na cílovou scénu

            // Po naètení scény správnì umístí hráèe
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (scene.buildIndex == targetSceneIndex)
                {
                    // Najdi vstupní bod ve druhé scénì (napø. objekt s tagem "EntryPoint")
                    GameObject entryPoint = GameObject.FindGameObjectWithTag("EntryPoint");
                    if (entryPoint != null)
                    {
                        player.transform.position = entryPoint.transform.position; // Pøemísti hráèe
                    }

                    SceneManager.sceneLoaded -= null; // Odebere posluchaè pro tuto událost
                }
            };
        }
    }
}