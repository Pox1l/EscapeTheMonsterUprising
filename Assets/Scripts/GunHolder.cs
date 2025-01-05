using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [SerializeField] private Transform gun; // Reference na Transform zbran�
    [SerializeField] private Transform gunHoldPoint; // Bod, kam se zbra� p�ipoj�
    [SerializeField] private Vector2 offset; // Posunut� zbran� od bodu p�ipojen�
    [SerializeField] private bool followMouse = true; // Nastavit sm��ov�n� zbran� podle my�i
    [SerializeField] private SpriteRenderer playerSprite; // Renderer hr��e pro kontrolu vrstvy
    [SerializeField] private SpriteRenderer gunSprite; // Renderer zbran�

    private Vector3 lastPosition;
    private bool isIdle;
    private float idleThreshold = 0.01f; // Prahov� hodnota pro detekci klidu
    private float idleTime = 0.2f; // �as, po kter�m se hr�� pova�uje za "idle"
    private float idleTimer = 0f; // �asova� pro idle stav

    private void Start()
    {
        lastPosition = transform.position; // Ulo�en� po��te�n� pozice hr��e
    }

    private void Update()
    {
        if (gun != null && gunHoldPoint != null)
        {
            // Nastaven� pozice zbran� podle bodu p�ipojen� a offsetu
            gun.position = gunHoldPoint.position + (Vector3)offset;

            // Detekce sm�ru pohybu
            Vector3 movementDirection = transform.position - lastPosition;

            if (movementDirection.magnitude < idleThreshold)
            {
                // Pokud se hr�� neh�be, za�neme odpo��t�vat idle �as
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleTime)
                {
                    isIdle = true;
                }
            }
            else
            {
                // Hr�� se h�be, resetujeme idle stav
                idleTimer = 0f;
                isIdle = false;

                if (movementDirection.y > idleThreshold)
                {
                    // Hr�� b�� nahoru, zbra� se skryje
                    gunSprite.sortingOrder = playerSprite.sortingOrder - 1;
                }
                else if (movementDirection.y < -idleThreshold)
                {
                    // Hr�� b�� dol�, zbra� je viditeln�
                    gunSprite.sortingOrder = playerSprite.sortingOrder + 1;
                }
            }

            if (isIdle)
            {
                // Pokud je hr�� v klidu, zbra� se vr�t� p�ed n�j
                gunSprite.sortingOrder = playerSprite.sortingOrder + 1;
            }

            if (followMouse)
            {
                // Sm�r k my�i
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mousePosition - gun.position;
                direction.z = 0; // Ignorujeme osu Z v 2D

                // Oto�en� zbran�
                gun.right = direction;

                // Zrcadlen� zbran� na ose Y, pokud je my� nalevo
                if (mousePosition.x < transform.position.x)
                {
                    gun.localScale = new Vector3(1, -1, 1); // Zrcadlen� na ose Y
                }
                else
                {
                    gun.localScale = new Vector3(1, 1, 1); // Norm�ln� orientace
                }
            }

            lastPosition = transform.position; // Aktualizace posledn� pozice
        }
    }

    public void AttachGun(Transform newGun)
    {
        // P�ipojen� nov� zbran�
        gun = newGun;
        gun.SetParent(gunHoldPoint); // Zajist�, �e zbra� bude v�dy dr�ena
        gunSprite = gun.GetComponent<SpriteRenderer>(); // Z�sk�n� rendereru zbran�
    }

    public void DetachGun()
    {
        // Odpojen� zbran�
        if (gun != null)
        {
            gun.SetParent(null); // Uvoln� zbra� z dr��ku
            gun = null;
            gunSprite = null;
        }
    }
}
