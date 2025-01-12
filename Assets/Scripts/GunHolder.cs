using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [SerializeField] private Transform gunHoldPoint; // Bod pøipojení zbranì
    [SerializeField] private Vector2 offset; // Posunutí zbranì
    private Transform currentGun; // Aktuální zbraò
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
        Debug.Log("GunHolder instance byla inicializována.");
    }

    private void Start()
    {
        EnsureGunHoldPointExists(); // Ujistíme se, že máme pøiøazený gunHoldPoint
        AssignMainCamera(); // Inicializace kamery
    }

    private void Update()
    {
        if (mainCamera == null)
        {
            AssignMainCamera(); // Obnoví kameru, pokud byla zmìnìna scéna
        }

        if (gunHoldPoint == null)
        {
            EnsureGunHoldPointExists(); // Obnoví gunHoldPoint, pokud byl ztracen
        }

        if (currentGun != null)
        {
            RotateGunTowardsMouse(); // Otoèení zbranì za myší
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
        GameObject newGun = Instantiate(gunPrefab, gunHoldPoint.position + (Vector3)offset, Quaternion.identity);
        newGun.transform.SetParent(gunHoldPoint);
        currentGun = newGun.transform;
    }

    public void DetachGun()
    {
        // Odpojí aktuální zbraò
        if (currentGun != null)
        {
            Destroy(currentGun.gameObject);
            currentGun = null;
        }
    }

    private void RotateGunTowardsMouse()
    {
        if (mainCamera == null || gunHoldPoint == null) return;

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); // Získání pozice myši
        mousePosition.z = 0; // Zajištìní, že je myš na stejné úrovni jako zbraò

        Vector3 direction = (mousePosition - gunHoldPoint.position).normalized; // Smìr od zbranì k myši

        // Nastavení rotace zbranì
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentGun.rotation = Quaternion.Euler(0, 0, angle);

        // Zrcadlení zbranì podle pozice myši
        if (mousePosition.x < transform.position.x)
        {
            currentGun.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            currentGun.localScale = new Vector3(1, 1, 1);
        }
    }

    private void AssignMainCamera()
    {
        mainCamera = Camera.main; // Nastaví hlavní kameru
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found. Please ensure there is a camera tagged as 'MainCamera'.");
        }
    }

    private void EnsureGunHoldPointExists()
    {
        if (gunHoldPoint == null)
        {
            GameObject foundObject = GameObject.Find("GunHolder"); // Najde objekt podle jména
            if (foundObject != null)
            {
                gunHoldPoint = foundObject.transform;
                Debug.Log("GunHoldPoint assigned successfully.");
            }
            else
            {
                Debug.LogError("GunHoldPoint not found in the scene. Please ensure it exists.");
            }
        }
    }
}
