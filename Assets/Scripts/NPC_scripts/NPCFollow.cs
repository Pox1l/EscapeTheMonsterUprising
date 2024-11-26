using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    public Transform player; // Odkaz na hr��e
    public float followDistance = 2.0f; // Vzd�lenost, na kterou se NPC zastav�
    public float speed = 2.0f; // Rychlost pohybu NPC
    public Animator animator; // P�ipojen� Animator pro ovl�d�n� animac�
    private bool isFollowing = false; // Indikuje, zda NPC sleduje hr��e

    void Update()
    {
        if (isFollowing && player != null) // Zkontroluj, zda m� NPC koho sledovat
        {
            // Spo��tej sm�r k hr��i
            Vector2 direction = player.position - transform.position;

            if (direction.magnitude > followDistance) // Pokud je hr�� mimo zadanou vzd�lenost
            {
                // Pohyb sm�rem k hr��i
                Vector2 newPosition = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z); // Zachovej osu Z

                // Nastaven� animace podle sm�ru pohybu
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
                // Pokud je NPC bl�zko hr��e, p�ejdi do Idle stavu
                if (animator != null)
                {
                    animator.SetFloat("Speed", 0f); // Rychlost pohybu 0
                    animator.SetBool("IsMoving", false);
                }
            }
        }
        else
        {
            // Pokud NPC nesleduje hr��e, p�ejdi do Idle stavu
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f); // Rychlost pohybu 0
                animator.SetBool("IsMoving", false);
            }
        }
    }

    // Ve�ejn� metoda pro nastaven� stavu sledov�n�
    public void SetFollowing(bool following)
    {
        isFollowing = following;
    }
}
