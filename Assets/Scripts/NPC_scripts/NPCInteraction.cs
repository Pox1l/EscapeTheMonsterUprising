using UnityEngine;
using UnityEngine.UI; // Pro pr�ci s UI

public class NPCInteraction : MonoBehaviour
{
    public GameObject pressEIcon; // UI ikona pro "Press E"
    public float interactionDistance = 2.0f; // Maxim�ln� vzd�lenost pro interakci
    private Transform player; // Odkaz na hr��e

    void Start()
    {
        // Najdi hr��e podle tagu (p�edpokl�d� se, �e hr�� m� tag "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Skryj ikonu p�i spu�t�n�
        if (pressEIcon != null)
        {
            pressEIcon.SetActive(false);
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Spo��tej vzd�lenost mezi hr��em a NPC
            float distance = Vector2.Distance(player.position, transform.position);

            // Pokud je hr�� dostate�n� bl�zko, zobraz ikonu
            if (distance <= interactionDistance)
            {
                if (pressEIcon != null)
                {
                    pressEIcon.SetActive(true);
                }

                // Zkontroluj, zda hr�� stiskl kl�vesu E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    InteractWithNPC();
                }
            }
            else
            {
                // Skryj ikonu, pokud je hr�� mimo dosah
                if (pressEIcon != null)
                {
                    pressEIcon.SetActive(false);
                }
            }
        }
    }

    void InteractWithNPC()
    {
        // Logika interakce s NPC (nap�. otev�en� dialogu)
        Debug.Log("Interakce s NPC!");
    }

}
    