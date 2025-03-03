using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Transform target; // Cíl (hráè)
    private NavMeshAgent agent; // NavMeshAgent pro pathfinding
    private Animator animator; // Animator pro pohyb animací

    [SerializeField] private float moveSpeed = 2f; // Nastavitelná rychlost v Inspectoru

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("Chybí NavMeshAgent na " + gameObject.name);
            return;
        }

        // Umožní správné fungování v 2D
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Nastavení rychlosti podle Inspectoru
        agent.speed = moveSpeed;

        FindPlayer();
    }

    void Update()
    {
        if (target == null)
        {
            FindPlayer();
            return;
        }

        // Nastaví cíl pro NavMeshAgent
        agent.SetDestination(target.position);

        // Aktualizace animací
        Vector2 direction = agent.velocity.normalized;
        float currentSpeed = agent.velocity.magnitude;

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", currentSpeed);
    }

    // Najde hráèe podle tagu "Player"
    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    // Metoda pro zmìnu rychlosti bìhem hry
    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
        agent.speed = newSpeed;
    }
}
