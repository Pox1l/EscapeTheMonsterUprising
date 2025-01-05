using UnityEngine;
using TMPro; // Required for TextMeshPro

public class NPCFollower_withAnim : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the Inspector
    public float followSpeed = 3f; // Speed at which NPC follows the player
    public Vector2 rightTopOffset = new Vector2(1f, 1f); // Offset for the right-top position
    public Vector2 leftTopOffset = new Vector2(-1f, 1f); // Offset for the left-top position
    private Vector2 assignedOffset; // Dynamically assigned offset when starting to follow
    private bool isFollowing = false;
    private bool inRange = false;

    private Animator animator; // Reference to the Animator component

    public static int followingNPCCount = 0; // Tracks the number of NPCs currently following the player
    private const int maxFollowingNPCs = 2; // Maximum number of NPCs that can follow the player

    public TextMeshProUGUI npcCountText; // TextMeshPro component for displaying the count of following NPCs

    private void Start()
    {
        animator = GetComponent<Animator>();
        FindPlayer(); // Try to find the player at the start
        UpdateNPCCountUI(); // Update UI at the start
    }

    private void Update()
    {
        // If player is null, try to find them
        if (player == null)
        {
            FindPlayer();
            return;
        }

        // Toggle following state when pressing 'E' in range
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isFollowing)
            {
                StopFollowing();
            }
            else if (followingNPCCount < maxFollowingNPCs)
            {
                StartFollowing();
            }
            else
            {
                Debug.Log("Maximum number of NPCs are already following the player.");
            }
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
            Vector2 targetPosition = (Vector2)player.position + assignedOffset;
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            float speed = (targetPosition - (Vector2)transform.position).magnitude;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);

            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Speed", speed);
        }
    }

    private void StartFollowing()
    {
        if (followingNPCCount == 0)
        {
            assignedOffset = rightTopOffset;
        }
        else if (followingNPCCount == 1)
        {
            assignedOffset = leftTopOffset;
        }
        else
        {
            Debug.LogWarning("Unexpected follow count. No offset assigned.");
            return;
        }

        isFollowing = true;
        followingNPCCount++;
        UpdateNPCCountUI();
        Debug.Log($"NPC started following. Assigned offset: {assignedOffset}. Total following NPCs: {followingNPCCount}");
    }

    private void StopFollowing()
    {
        isFollowing = false;
        assignedOffset = Vector2.zero;
        followingNPCCount--;
        UpdateNPCCountUI();
        Debug.Log("NPC stopped following. Total following NPCs: " + followingNPCCount);
    }

    private void UpdateNPCCountUI()
    {
        if (npcCountText != null)
        {
            npcCountText.text = $"Following NPCs: {followingNPCCount} / {maxFollowingNPCs}";
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

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            Debug.Log("Player found and assigned to NPC.");
        }
        else
        {
            Debug.LogWarning("Player not found in the scene.");
        }
    }

    private void OnDestroy()
    {
        if (isFollowing)
        {
            followingNPCCount--;
            UpdateNPCCountUI();
            Debug.Log("NPC destroyed. Total following NPCs: " + followingNPCCount);
        }
    }
}
