    using UnityEngine;
    using UnityEngine.AI;

    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        private Transform player;         // Hráè
        private Transform target;         // Aktuální cíl (hráè nebo NPC)
        private NavMeshAgent agent;       // NavMeshAgent pro pohyb
        private Animator animator;        // Animator pro pohybové animace

        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float npcAggroRange = 3f; // Do jaké vzdálenosti monstrum reaguje na NPC

    private float searchCooldown = 0.2f;
    private float searchTimer = 0f;

    void Start()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

            if (agent == null)
            {
                Debug.LogError("Chybí NavMeshAgent na objektu " + gameObject.name);
                return;
            }

            //  DÙLEŽITÉ pro 2D NavMesh (NavMeshPlus)
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.speed = moveSpeed;

            FindPlayer();
        }

        void Update()
        {
            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0f)
            {
                FindClosestTarget();
                searchTimer = searchCooldown;
            }

        if (target != null)
            {
                agent.SetDestination(target.position);

                // Animace
                Vector2 velocity = agent.velocity;
                Vector2 direction = velocity.normalized;
                float speed = velocity.magnitude;

                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
                animator.SetFloat("Speed", speed);
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }

        void FindPlayer()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        void FindClosestTarget()
        {
            GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
            Transform closestNpc = null;
            float closestDistance = npcAggroRange;

            foreach (GameObject npc in npcs)
            {
                float distance = Vector2.Distance(transform.position, npc.transform.position);
                if (distance < closestDistance)
                {
                    closestNpc = npc.transform;
                    closestDistance = distance;
                }
            }

            // Pokud je v dosahu NPC, sleduj ji. Jinak hráèe.
            target = closestNpc != null ? closestNpc : player;
        }

        public Transform GetCurrentTarget()
        {
            return target;
        }
    }
