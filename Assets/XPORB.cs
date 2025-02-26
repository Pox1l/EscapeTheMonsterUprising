using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpAmount = 10;
    public float moveSpeed = 5f;
    public float pickupRange = 0.5f;
    public float rangeToActivate = 5f;

    private Transform player;
    private bool shouldMoveToPlayer = false;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Ujisti se, ûe hr·Ë m· tag 'Player'.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) <= rangeToActivate)
            {
                shouldMoveToPlayer = true;
            }

            if (shouldMoveToPlayer)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"XP Orb kolidoval s: {other.gameObject.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("XP Orb dotek hr·Ëe!");

            if (XPManager.instance != null)
            {
                
                XPManager.instance.AddXP(xpAmount);
                Debug.Log($"Hr·Ë zÌskal {xpAmount} XP!");
            }
            else
            {
                Debug.LogError("XPManager nenalezen!");
            }

            Destroy(gameObject);
        }
    }
}
