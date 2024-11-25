using UnityEngine;
using UnityEngine.UI; // Pro práci s UI

public class NPCInteraction : MonoBehaviour
{
    public GameObject pressEIcon; // UI ikona pro "Press E"
    public float interactionDistance = 2.0f; // Maximální vzdálenost pro interakci
    private Transform player; // Odkaz na hráèe

    void Start()
    {
        // Najdi hráèe podle tagu (pøedpokládá se, že hráè má tag "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Skryj ikonu pøi spuštìní
        if (pressEIcon != null)
        {
            pressEIcon.SetActive(false);
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Spoèítej vzdálenost mezi hráèem a NPC
            float distance = Vector2.Distance(player.position, transform.position);

            // Pokud je hráè dostateènì blízko, zobraz ikonu
            if (distance <= interactionDistance)
            {
                if (pressEIcon != null)
                {
                    pressEIcon.SetActive(true);
                }

                // Zkontroluj, zda hráè stiskl klávesu E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    InteractWithNPC();
                }
            }
            else
            {
                // Skryj ikonu, pokud je hráè mimo dosah
                if (pressEIcon != null)
                {
                    pressEIcon.SetActive(false);
                }
            }
        }
    }

    void InteractWithNPC()
    {
        // Logika interakce s NPC (napø. otevøení dialogu)
        Debug.Log("Interakce s NPC!");
    }

}
    