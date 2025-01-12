using UnityEngine;
using UnityEngine.UI; // Pro zobrazení UI textu
using UnityEngine.SceneManagement; // Pro pøepínání scén

public class BunkerExit : MonoBehaviour
{
    public GameObject interactText; // Text, který se zobrazí (napø. "Stiskni E pro výstup")
    public Vector3 fallbackPosition = new Vector3(0, 0, 0); // Pozice, kam hráè spadne, pokud EntryPoint neexistuje
    private bool playerInRange = false; // Sleduje, zda je hráè v oblasti

    void Start()
    {
        if (interactText != null)
            interactText.SetActive(false); // Skryje text pøi startu
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Nastav aktuální pozici hráèe pro novou scénu
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(2); // Pøepne na scénu venku
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
            if (interactText != null)
                interactText.SetActive(false); // Skryje text
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Odpoj událost po naètení scény

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject entryPoint = GameObject.FindGameObjectWithTag("EntryPoint");

        if (player != null && entryPoint != null)
        {
            // Pøesuò hráèe na EntryPoint
            player.transform.position = entryPoint.transform.position;
        }
        else if (player != null)
        {
            // Pokud EntryPoint neexistuje, nastav fallback pozici
            player.transform.position = fallbackPosition;
            Debug.LogWarning("EntryPoint nebyl nalezen, hráè pøesunut na fallback pozici.");
        }
    }
}
