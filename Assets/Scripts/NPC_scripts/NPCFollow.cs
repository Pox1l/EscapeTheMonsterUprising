using UnityEngine;

public class NPCFollowWithDirectionalAnimation : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the Inspector
    public float followSpeed = 3f; // Speed at which NPC follows the player
    private bool isFollowing = false;
    private bool inRange = false;

    private Animator animator; // Reference to the Animator component

    private void Start()
    {
        // Get the Animator component attached to the NPC
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Toggle following state when pressing 'E' in range
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            isFollowing = !isFollowing;
            Debug.Log("Following toggled: " + isFollowing);
        }

        // Follow player and update animation parameters if following
        if (isFollowing)
        {
            FollowPlayer();
        }
        else
        {
            animator.SetFloat("Speed", 0); // Stop animation when not following
        }
    }

    private void FollowPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float speed = direction.magnitude;

            // Move NPC towards player
            transform.position = Vector2.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);

            // Update Animator parameters
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Speed", speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            Debug.Log("Player entered interaction range.");
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            Debug.Log("Player exited interaction range.");
            inRange = false;
        }
    }
}
