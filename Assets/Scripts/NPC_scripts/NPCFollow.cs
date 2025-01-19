using UnityEngine;
using TMPro;

public class NPCFollow : MonoBehaviour
{
    public Transform player; // Hráèùv Transform
    public float followSpeed = 3f; // Rychlost sledování
    public Vector2 rightTopOffset = new Vector2(1f, 1f);
    public Vector2 leftTopOffset = new Vector2(-1f, 1f);
    private Vector2 assignedOffset;
    private bool isFollowing = false;
    private bool inRange = false;

    private Animator animator;
    public static int followingNPCCount = 0;
    private const int maxFollowingNPCs = 2;

    public TextMeshProUGUI npcCountText;
    private HatchManager hatchManager; // Odkaz na HatchManager

    private void Start()
    {
        animator = GetComponent<Animator>();
        FindPlayer();
        UpdateNPCCountUI();
    }

    private void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

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

        if (isFollowing)
        {
            FollowPlayer();
        }
        else
        {
            animator.SetFloat("Speed", 0);
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

        // Pokud NPC vstoupí do bunkru
        if (collider2D.CompareTag("Hatch") && isFollowing)
        {
            RescueNPC();
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

    private void RescueNPC()
    {
        Debug.Log($"NPC {gameObject.name} zachránìno!");
        isFollowing = false; // Pøestane sledovat hráèe
        followingNPCCount--; // Snížení poètu sledujících NPC
        UpdateNPCCountUI(); // Aktualizace UI

        hatchManager = FindObjectOfType<HatchManager>();
        if (hatchManager != null)
        {
            hatchManager.AddRescuedNPC();
        }

        // Pøidání penìz hráèi po zachránìní NPC
        PlayerMoney.Instance.AddMoney(50); // Pøedpokládané množství penìz, které se pøidá za zachránìné NPC

        Destroy(gameObject); // NPC zmizí
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
