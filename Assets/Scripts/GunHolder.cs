using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [SerializeField] private Transform gunHoldPoint; // Bod pøipojení zbranì
    [SerializeField] private Vector2 offset; // Posunutí zbranì
    private Gun currentGun; // Tøída reprezentující zbraò
    private Camera mainCamera; // Hlavní kamera pro získání pozice myši

    public static GunHolder Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Zniè duplicitní instance
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Zajistí, že objekt pøežije pøechod mezi scénami
    }

    private void Start()
    {
        AssignMainCamera(); // Inicializace kamery
        if (gunHoldPoint == null)
        {
            Debug.LogError("GunHoldPoint is not assigned in the Inspector!");
        }
    }

    private void Update()
    {
        if (mainCamera == null)
        {
            AssignMainCamera(); // Obnoví kameru, pokud byla zmìnìna scéna
        }

        if (currentGun != null && mainCamera != null)
        {
            RotateGunTowardsMouse(); // Otoèení zbranì za myší

            // Støelba pøi stisknutí tlaèítka myši
            if (Input.GetMouseButton(0)) // Levé tlaèítko myši
            {
                currentGun.Shoot();
            }
        }
    }

    public void AttachGun(GameObject gunPrefab)
    {
        DetachGun(); // Odstraò starou zbraò

        if (gunHoldPoint == null)
        {
            Debug.LogError("GunHoldPoint is null. Cannot attach a gun.");
            return;
        }

        // Vytvoø novou zbraò a pøipoj ji
        GameObject newGunObject = Instantiate(gunPrefab, gunHoldPoint.position + (Vector3)offset, Quaternion.identity);
        newGunObject.transform.SetParent(gunHoldPoint);

        // Pøiøaï komponentu Gun ke GunHolderu
        currentGun = newGunObject.GetComponent<Gun>();
        if (currentGun == null)
        {
            Debug.LogError("The attached gun prefab does not have a Gun component.");
        }
    }

    public void DetachGun()
    {
        // Odpojí aktuální zbraò
        if (currentGun != null)
        {
            Destroy(currentGun.gameObject);
            Debug.Log("Gun detached.");
            currentGun = null;
        }
    }

    private void RotateGunTowardsMouse()
    {
        if (mainCamera == null || currentGun == null)
        {
            Debug.LogError("Camera or CurrentGun is null!");
            return;
        }

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - gunHoldPoint.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentGun.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (mousePosition.x < gunHoldPoint.position.x)
        {
            currentGun.transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            currentGun.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void AssignMainCamera()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found. Please ensure there is a camera tagged as 'MainCamera'.");
        }
    }
}
