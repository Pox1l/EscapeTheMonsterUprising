using UnityEngine;
using UnityEngine.UI; // Pro zobrazen� UI textu
using UnityEngine.SceneManagement; // Pro p�ep�n�n� sc�n

public class BunkerExit : MonoBehaviour
{
    public GameObject interactText; // Text, kter� se zobraz� (nap�. "Stiskni E pro v�stup")
    public Vector3 fallbackPosition = new Vector3(0, 0, 0); // Pozice, kam hr�� spadne, pokud EntryPoint neexistuje
    private bool playerInRange = false; // Sleduje, zda je hr�� v oblasti

    void Start()
    {
        if (interactText != null)
            interactText.SetActive(false); // Skryje text p�i startu
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Nastav aktu�ln� pozici hr��e pro novou sc�nu
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(2); // P�epne na sc�nu venku
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
            if (interactText != null)
                interactText.SetActive(false); // Skryje text
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Odpoj ud�lost po na�ten� sc�ny

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject entryPoint = GameObject.FindGameObjectWithTag("EntryPoint");

        if (player != null && entryPoint != null)
        {
            // P�esu� hr��e na EntryPoint
            player.transform.position = entryPoint.transform.position;
        }
        else if (player != null)
        {
            // Pokud EntryPoint neexistuje, nastav fallback pozici
            player.transform.position = fallbackPosition;
            Debug.LogWarning("EntryPoint nebyl nalezen, hr�� p�esunut na fallback pozici.");
        }
    }
}
