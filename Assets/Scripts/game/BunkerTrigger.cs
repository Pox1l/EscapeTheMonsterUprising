using UnityEngine;
using UnityEngine.UI; // Pro zobrazen� UI textu
using UnityEngine.SceneManagement; // Pro p�ep�n�n� sc�n

public class BunkerTrigger : MonoBehaviour
{
    public GameObject interactText; // Text, kter� se zobraz� (nap�. "Dr� E pro vstup")
    public float holdTime = 2f; // Doba dr�en� E pro p�echod
    private float holdProgress = 0f; // Sledov�n� dr�en� kl�vesy

    private bool playerInRange = false; // Sleduje, zda je hr�� v oblasti

    // ID sc�ny, kam se p�ech�z�
    public int targetSceneIndex = 1; // ��slo sc�ny (nap�. 1 = GameInside)

    void Start()
    {
        if (interactText != null)
            interactText.SetActive(false); // Schov� text p�i startu
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
            holdProgress = Mathf.Max(0, holdProgress - Time.deltaTime); // Resetuje postup, pokud E nen� dr�eno
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactText != null)
                interactText.SetActive(true); // Zobraz� text
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            holdProgress = 0f; // Resetuje postup dr�en�
            if (interactText != null)
                interactText.SetActive(false); // Skryje text
        }
    }

    private void MovePlayerToScene()
    {
        // Najde hr��e
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            DontDestroyOnLoad(player); // Zajist�, �e hr�� nezmiz�
            SceneManager.LoadScene(targetSceneIndex); // P�epne na c�lovou sc�nu

            // Po na�ten� sc�ny spr�vn� um�st� hr��e
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (scene.buildIndex == targetSceneIndex)
                {
                    // Najdi vstupn� bod ve druh� sc�n� (nap�. objekt s tagem "EntryPoint")
                    GameObject entryPoint = GameObject.FindGameObjectWithTag("EntryPoint");
                    if (entryPoint != null)
                    {
                        player.transform.position = entryPoint.transform.position; // P�em�sti hr��e
                    }

                    SceneManager.sceneLoaded -= null; // Odebere poslucha� pro tuto ud�lost
                }
            };
        }
    }
}