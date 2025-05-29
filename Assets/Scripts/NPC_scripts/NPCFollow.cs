using System;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class NPCFollow : MonoBehaviour
{
    [Header("General")]
    public Transform player;
    public float minWanderSpeed = 1.5f;
    public float maxWanderSpeed = 3.5f;
    public float wanderRadius = 5f;
    public float minWanderInterval = 2f;
    public float maxWanderInterval = 5f;

    [Header("Offsets")]
    public Vector2 rightTopOffset = new Vector2(1f, 1f);
    public Vector2 leftTopOffset = new Vector2(-1f, 1f);

    [Header("UI")]
    public TextMeshProUGUI npcCountText;
    public TextMeshProUGUI npcRemainingText;
    public static int followingNPCCount = 0;
    private const int maxFollowingNPCs = 2;

    public static int totalNPCs = 0;
    public static event Action OnNPCCountChanged;

    [Header("Particles")]
    [SerializeField] private ParticleSystem exportParticle;

    public event Action onRescued;
    public event Action onRemoved;
    [NonSerialized] public NPCSpawner Spawner;

    private Animator animator;
    private NavMeshAgent agent;
    private Vector2 assignedOffset;
    private bool isFollowing = false;
    private bool inRange = false;
    private float wanderTimer;
    private float currentWanderInterval;
    private HatchManager hatchManager;
    private bool isRemoved = false; //  NOVĚ PŘIDÁNO

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (Spawner == null)
        {
            NPCSpawner foundSpawner = FindObjectOfType<NPCSpawner>();
            if (foundSpawner != null)
            {
                foundSpawner.Register(this);
            }
        }
    }

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        FindPlayer();
        UpdateNPCCountUI();

        OnNPCCountChanged += UpdateTotalNPCUI;
        UpdateTotalNPCUI();

        ResetWander();
    }

    private void Update()
    {
        if (player == null) { FindPlayer(); return; }

        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isFollowing) StopFollowing();
            else if (followingNPCCount < maxFollowingNPCs) StartFollowing();
        }

        if (isFollowing)
        {
            Vector2 targetPos = (Vector2)player.position + assignedOffset;
            agent.SetDestination(targetPos);
        }
        else
        {
            Wander();
        }

        UpdateAnimator();
    }

    private void Wander()
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f)
        {
            Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(1f, wanderRadius);
            Vector2 targetPosition = (Vector2)transform.position + randomDirection;

            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                agent.speed = UnityEngine.Random.Range(minWanderSpeed, maxWanderSpeed);
                agent.SetDestination(hit.position);
            }

            ResetWander();
        }
    }

    private void ResetWander()
    {
        wanderTimer = UnityEngine.Random.Range(minWanderInterval, maxWanderInterval);
    }

    private void UpdateAnimator()
    {
        Vector2 vel = agent.velocity;
        float spd = vel.magnitude;
        Vector2 dir = vel.normalized;

        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.y);
        animator.SetFloat("Speed", spd);
    }

    private void StartFollowing()
    {
        assignedOffset = followingNPCCount == 0 ? rightTopOffset : leftTopOffset;
        isFollowing = true;
        followingNPCCount++;
        UpdateNPCCountUI();
    }

    private void StopFollowing()
    {
        isFollowing = false;
        assignedOffset = Vector2.zero;
        followingNPCCount--;
        UpdateNPCCountUI();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) inRange = true;
        if (col.CompareTag("Hatch") && isFollowing) RescueNPC();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player")) inRange = false;
    }

    private void RescueNPC()
    {
        if (isRemoved) return; //  chrání před dvojím zavoláním
        isRemoved = true;

        if (exportParticle) Instantiate(exportParticle, transform.position, Quaternion.identity);

        isFollowing = false;
        followingNPCCount--;
        UpdateNPCCountUI();
        onRescued?.Invoke();

        if (hatchManager == null)
            hatchManager = FindObjectOfType<HatchManager>();
        hatchManager?.AddRescuedNPC();

        AudioManager.instance.PlayNPCRescue();
        PlayerMoney.Instance.AddMoney(50);
        Player_XP.Instance.AddXP(10);

        totalNPCs--;
        OnNPCCountChanged?.Invoke();

        Destroy(gameObject);
    }

    private void FindPlayer()
    {
        if (GameObject.FindGameObjectWithTag("Player") is { } pl)
            player = pl.transform;
    }

    private void UpdateNPCCountUI()
    {
        if (npcCountText)
            npcCountText.text = $"Following NPCs: {followingNPCCount} / {maxFollowingNPCs}";
    }

    private void UpdateTotalNPCUI()
    {
        if (npcRemainingText)
            npcRemainingText.text = $"NPC Remaining: {totalNPCs}";
    }

    private void OnDestroy()
    {
        if (isRemoved) return; //  už byl odstraněn/zachráněn
        isRemoved = true;

        if (isFollowing)
        {
            followingNPCCount--;
            UpdateNPCCountUI();
        }

        totalNPCs--;
        OnNPCCountChanged?.Invoke();
        OnNPCCountChanged -= UpdateTotalNPCUI;

        onRemoved?.Invoke();
    }
}
