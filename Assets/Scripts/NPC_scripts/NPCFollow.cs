using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float followDistance = 2.0f; // Distance at which the NPC will stop
    public float speed = 2.0f; // Movement speed of the NPC
    public Animator animator; // Animator to control NPC animations
    public float detectionRange = 5.0f; // Range within which the NPC detects the player

    private bool isFollowing = false; // Indicates whether the NPC is following the player

    void Update()
    {
        if (player != null)
        {
            if (InRange()) // Check if the player is in range
            {
                if (isFollowing)
                {
                    FollowPlayer();
                }
            }
            else
            {
                StopFollowing();
            }
        }
    }

    // Method to check if the player is within detection range
    private bool InRange()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= detectionRange;
    }

    private void FollowPlayer()
    {
        // Calculate direction towards the player
        Vector2 direction = player.position - transform.position;

        if (direction.magnitude > followDistance) // If the player is beyond followDistance
        {
            // Move towards the player
            Vector2 newPosition = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z); // Maintain Z-axis

            // Update animation based on movement
            if (animator != null)
            {
                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
                animator.SetFloat("Speed", direction.magnitude);
            }
        }
        else
        {
            StopFollowing();
        }
    }

    private void StopFollowing()
    {
        // Stop animations when the NPC is idle
        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    // Public method to toggle following
    public void SetFollowing(bool following)
    {
        isFollowing = following;
    }
}
