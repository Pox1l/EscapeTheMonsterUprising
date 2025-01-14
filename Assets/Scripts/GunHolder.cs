using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [SerializeField] private Transform gunHoldPoint; // Bod p�ipojen� zbran�
    [SerializeField] private Vector2 offset; // Posunut� zbran�
    private Gun currentGun; // T��da reprezentuj�c� zbra�
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
            AssignMainCamera(); // Obnov� kameru, pokud byla zm�n�na sc�na
        }

        if (currentGun != null && mainCamera != null)
        {
            RotateGunTowardsMouse(); // Oto�en� zbran� za my��

            // St�elba p�i stisknut� tla��tka my�i
            if (Input.GetMouseButton(0)) // Lev� tla��tko my�i
            {
                currentGun.Shoot();
            }
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
        GameObject newGunObject = Instantiate(gunPrefab, gunHoldPoint.position + (Vector3)offset, Quaternion.identity);
        newGunObject.transform.SetParent(gunHoldPoint);

        // P�i�a� komponentu Gun ke GunHolderu
        currentGun = newGunObject.GetComponent<Gun>();
        if (currentGun == null)
        {
            Debug.LogError("The attached gun prefab does not have a Gun component.");
        }
    }

    public void DetachGun()
    {
        // Odpoj� aktu�ln� zbra�
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
