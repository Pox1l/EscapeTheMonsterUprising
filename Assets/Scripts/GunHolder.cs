using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [SerializeField] private Transform gun; // Reference na Transform zbranì
    [SerializeField] private Transform gunHoldPoint; // Bod, kam se zbraò pøipojí
    [SerializeField] private Vector2 offset; // Posunutí zbranì od bodu pøipojení
    [SerializeField] private bool followMouse = true; // Nastavit smìøování zbranì podle myši
    [SerializeField] private SpriteRenderer playerSprite; // Renderer hráèe pro kontrolu vrstvy
    [SerializeField] private SpriteRenderer gunSprite; // Renderer zbranì

    private Vector3 lastPosition;
    private bool isIdle;
    private float idleThreshold = 0.01f; // Prahová hodnota pro detekci klidu
    private float idleTime = 0.2f; // Èas, po kterém se hráè považuje za "idle"
    private float idleTimer = 0f; // Èasovaè pro idle stav

    private void Start()
    {
        lastPosition = transform.position; // Uložení poèáteèní pozice hráèe
    }

    private void Update()
    {
        if (gun != null && gunHoldPoint != null)
        {
            // Nastavení pozice zbranì podle bodu pøipojení a offsetu
            gun.position = gunHoldPoint.position + (Vector3)offset;

            // Detekce smìru pohybu
            Vector3 movementDirection = transform.position - lastPosition;

            if (movementDirection.magnitude < idleThreshold)
            {
                // Pokud se hráè nehýbe, zaèneme odpoèítávat idle èas
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleTime)
                {
                    isIdle = true;
                }
            }
            else
            {
                // Hráè se hýbe, resetujeme idle stav
                idleTimer = 0f;
                isIdle = false;

                if (movementDirection.y > idleThreshold)
                {
                    // Hráè bìží nahoru, zbraò se skryje
                    gunSprite.sortingOrder = playerSprite.sortingOrder - 1;
                }
                else if (movementDirection.y < -idleThreshold)
                {
                    // Hráè bìží dolù, zbraò je viditelná
                    gunSprite.sortingOrder = playerSprite.sortingOrder + 1;
                }
            }

            if (isIdle)
            {
                // Pokud je hráè v klidu, zbraò se vrátí pøed nìj
                gunSprite.sortingOrder = playerSprite.sortingOrder + 1;
            }

            if (followMouse)
            {
                // Smìr k myši
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mousePosition - gun.position;
                direction.z = 0; // Ignorujeme osu Z v 2D

                // Otoèení zbranì
                gun.right = direction;

                // Zrcadlení zbranì na ose Y, pokud je myš nalevo
                if (mousePosition.x < transform.position.x)
                {
                    gun.localScale = new Vector3(1, -1, 1); // Zrcadlení na ose Y
                }
                else
                {
                    gun.localScale = new Vector3(1, 1, 1); // Normální orientace
                }
            }

            lastPosition = transform.position; // Aktualizace poslední pozice
        }
    }

    public void AttachGun(Transform newGun)
    {
        // Pøipojení nové zbranì
        gun = newGun;
        gun.SetParent(gunHoldPoint); // Zajistí, že zbraò bude vždy držena
        gunSprite = gun.GetComponent<SpriteRenderer>(); // Získání rendereru zbranì
    }

    public void DetachGun()
    {
        // Odpojení zbranì
        if (gun != null)
        {
            gun.SetParent(null); // Uvolní zbraò z držáku
            gun = null;
            gunSprite = null;
        }
    }
}
