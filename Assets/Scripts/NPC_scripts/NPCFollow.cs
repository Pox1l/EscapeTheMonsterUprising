using UnityEngine;
using UnityEngine.AI;
using TMPro;


public class NPCFollow : MonoBehaviour
{
    [Header("Particle")]
    [SerializeField] private ParticleSystem exportParticle;

    private ParticleSystem exportParticleInstance;

    public Transform player;
    public float followSpeed = 3f;
    public Vector2 rightTopOffset = new Vector2(1f, 1f);
    public Vector2 leftTopOffset = new Vector2(-1f, 1f);
    private Vector2 assignedOffset;

    private bool isFollowing = false;
    private bool inRange = false;

    private Animator animator;
    private NavMeshAgent agent;

    public static int followingNPCCount = 0;
    private const int maxFollowingNPCs = 2;

    public TextMeshProUGUI npcCountText;
    private HatchManager hatchManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = followSpeed;

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

        if (isFollowing && player != null)
        {
            Vector2 targetPos = (Vector2)player.position + assignedOffset;
            agent.SetDestination(targetPos);

            Vector2 velocity = agent.velocity;
            float speed = velocity.magnitude;
            Vector2 dir = velocity.normalized;

            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);
            animator.SetFloat("Speed", speed);
        }
        else
        {
            animator.SetFloat("Speed", 0);
            agent.ResetPath();
        }
    }

    private void StartFollowing()
    {
        assignedOffset = followingNPCCount == 0 ? rightTopOffset : leftTopOffset;
        isFollowing = true;
        followingNPCCount++;
        UpdateNPCCountUI();
        Debug.Log($"NPC started following. Offset: {assignedOffset}. Following: {followingNPCCount}");
    }

    private void StopFollowing()
    {
        isFollowing = false;
        assignedOffset = Vector2.zero;
        followingNPCCount--;
        UpdateNPCCountUI();
        Debug.Log("NPC stopped following.");
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            inRange = true;
        }

        if (collider2D.CompareTag("Hatch") && isFollowing)
        {
            RescueNPC();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    private void RescueNPC()
    {
        SpawnDamageParticle();
        Debug.Log($"NPC {gameObject.name} zachránìno!");
        isFollowing = false;
        followingNPCCount--;
        UpdateNPCCountUI();

        // Ujisti se, že hatchManager existuje
        if (hatchManager == null)
        {
            hatchManager = FindObjectOfType<HatchManager>();
            if (hatchManager == null)
            {
                Debug.LogError("HatchManager nebyl nalezen v scénì!");
                return; // Pøerušíme funkci, protože nemáme kam pøidat zachránìné NPC
            }
        }

        hatchManager.AddRescuedNPC();

        Destroy(gameObject);

        PlayerMoney.Instance.AddMoney(50);
        Player_XP.Instance.AddXP(10);
    }


    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void UpdateNPCCountUI()
    {
        if (npcCountText != null)
        {
            npcCountText.text = $"Following NPCs: {followingNPCCount} / {maxFollowingNPCs}";
        }
    }

    private void SpawnDamageParticle()
    {
        if (exportParticle != null)
        {
            exportParticleInstance = Instantiate(exportParticle, transform.position, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        if (isFollowing)
        {
            followingNPCCount--;
            UpdateNPCCountUI();
        }
    }
}
