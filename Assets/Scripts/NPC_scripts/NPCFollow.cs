using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    public Transform player; // Odkaz na hráèe
    public float followDistance = 2.0f; // Vzdálenost, na kterou se NPC zastaví
    public float speed = 2.0f; // Rychlost pohybu NPC
    public Animator animator; // Pøipojený Animator pro ovládání animací
    private bool isFollowing = false; // Indikuje, zda NPC sleduje hráèe

    void Update()
    {
        if (isFollowing && player != null) // Zkontroluj, zda má NPC koho sledovat
        {
            // Spoèítej smìr k hráèi
            Vector2 direction = player.position - transform.position;

            if (direction.magnitude > followDistance) // Pokud je hráè mimo zadanou vzdálenost
            {
                // Pohyb smìrem k hráèi
                Vector2 newPosition = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z); // Zachovej osu Z

                // Nastavení animace podle smìru pohybu
                if (animator != null)
                {
                    animator.SetFloat("Horizontal", direction.x);
                    animator.SetFloat("Vertical", direction.y);
                    animator.SetFloat("Speed", direction.magnitude);
                    animator.SetBool("IsMoving", true);
                }
            }
            else
            {
                // Pokud je NPC blízko hráèe, pøejdi do Idle stavu
                if (animator != null)
                {
                    animator.SetFloat("Speed", 0f); // Rychlost pohybu 0
                    animator.SetBool("IsMoving", false);
                }
            }
        }
        else
        {
            // Pokud NPC nesleduje hráèe, pøejdi do Idle stavu
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f); // Rychlost pohybu 0
                animator.SetBool("IsMoving", false);
            }
        }
    }

    // Veøejná metoda pro nastavení stavu sledování
    public void SetFollowing(bool following)
    {
        isFollowing = following;
    }
}
