using UnityEngine;
using UnityEngine.UI; // Pro zobrazení UI textu
using UnityEngine.SceneManagement; // Pro pøepínání scén

public class BunkerExit : MonoBehaviour
{
   
    public GameObject interactText; // Text, který se zobrazí (napø. "Stiskni E pro výstup")
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
            SceneManager.LoadScene(2); // Pøepne na scénu po stisku E
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
}
