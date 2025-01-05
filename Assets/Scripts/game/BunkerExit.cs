using UnityEngine;
using UnityEngine.UI; // Pro zobrazen� UI textu
using UnityEngine.SceneManagement; // Pro p�ep�n�n� sc�n

public class BunkerExit : MonoBehaviour
{
   
    public GameObject interactText; // Text, kter� se zobraz� (nap�. "Stiskni E pro v�stup")
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
            SceneManager.LoadScene(2); // P�epne na sc�nu po stisku E
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
}
