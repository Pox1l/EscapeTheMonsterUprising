using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [SerializeField] private Transform gunHoldPoint; // Bod p�ipojen� zbran�
    [SerializeField] private Vector2 offset; // Posunut� zbran�
    private Transform currentGun; // Aktu�ln� zbra�
    private Camera mainCamera; // Hlavn� kamera pro z�sk�n� pozice my�i

    public static GunHolder Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Zni� duplicitn� instance
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Zajist�, �e objekt p�e�ije p�echod mezi sc�nami
        Debug.Log("GunHolder instance byla inicializov�na.");
    }

    private void Start()
    {
        EnsureGunHoldPointExists(); // Ujist�me se, �e m�me p�i�azen� gunHoldPoint
        AssignMainCamera(); // Inicializace kamery
    }

    private void Update()
    {
        if (mainCamera == null)
        {
            AssignMainCamera(); // Obnov� kameru, pokud byla zm�n�na sc�na
        }

        if (gunHoldPoint == null)
        {
            EnsureGunHoldPointExists(); // Obnov� gunHoldPoint, pokud byl ztracen
        }

        if (currentGun != null)
        {
            RotateGunTowardsMouse(); // Oto�en� zbran� za my��
        }
    }

    public void AttachGun(GameObject gunPrefab)
    {
        DetachGun(); // Odstra� starou zbra�

        if (gunHoldPoint == null)
        {
            Debug.LogError("GunHoldPoint is null. Cannot attach a gun.");
            return;
        }

        // Vytvo� novou zbra� a p�ipoj ji
        GameObject newGun = Instantiate(gunPrefab, gunHoldPoint.position + (Vector3)offset, Quaternion.identity);
        newGun.transform.SetParent(gunHoldPoint);
        currentGun = newGun.transform;
    }

    public void DetachGun()
    {
        // Odpoj� aktu�ln� zbra�
        if (currentGun != null)
        {
            Destroy(currentGun.gameObject);
            currentGun = null;
        }
    }

    private void RotateGunTowardsMouse()
    {
        if (mainCamera == null || gunHoldPoint == null) return;

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); // Z�sk�n� pozice my�i
        mousePosition.z = 0; // Zaji�t�n�, �e je my� na stejn� �rovni jako zbra�

        Vector3 direction = (mousePosition - gunHoldPoint.position).normalized; // Sm�r od zbran� k my�i

        // Nastaven� rotace zbran�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentGun.rotation = Quaternion.Euler(0, 0, angle);

        // Zrcadlen� zbran� podle pozice my�i
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
        mainCamera = Camera.main; // Nastav� hlavn� kameru
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found. Please ensure there is a camera tagged as 'MainCamera'.");
        }
    }

    private void EnsureGunHoldPointExists()
    {
        if (gunHoldPoint == null)
        {
            GameObject foundObject = GameObject.Find("GunHolder"); // Najde objekt podle jm�na
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
