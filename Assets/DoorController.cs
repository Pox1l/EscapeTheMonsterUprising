using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject doorTilemap;       // Dveøe (tilemapa nebo jiný objekt)
    public GameObject interactText;      // UI text nebo ikona pro interakci
    public string doorID = "Door1";      // Unikátní ID dveøí
    private bool playerInRange = false;
    private bool isOpen = false;

    void Start()
    {
        // Naèti uložený stav dveøí
        isOpen = PlayerPrefs.GetInt(doorID, 0) == 1;

        if (isOpen)
            doorTilemap.SetActive(false);
        else
            doorTilemap.SetActive(true);

        // Skryj text na zaèátku
        if (interactText != null)
            interactText.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactText != null)
                interactText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactText != null)
                interactText.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpen && PlayerInventory.instance != null && PlayerInventory.instance.hasKey)
            {
                isOpen = true;
                doorTilemap.SetActive(false);
                PlayerPrefs.SetInt(doorID, 1);
                PlayerPrefs.Save();
                PlayerInventory.instance.hasKey = false;
            }
            else if (isOpen)
            {
                isOpen = false;
                doorTilemap.SetActive(true);
                PlayerPrefs.SetInt(doorID, 0);
                PlayerPrefs.Save();
            }

            // Skryj text po interakci (volitelné)
            if (interactText != null)
                interactText.SetActive(false);
        }
    }
}
