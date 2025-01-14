using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance { get; private set; }

    [SerializeField] private Transform gunHoldPoint; // Bod p�ipojen� zbran�
    private GameObject currentGun; // Aktu�ln� zbra� hr��e
    private string currentWeaponName; // Jm�no aktu�ln� zbran�

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // P�etrv� mezi sc�nami
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(currentWeaponName))
        {
            LoadWeapon(currentWeaponName); // Na�te ulo�enou zbra�
        }
    }

    private void Update()
    {
        if (currentGun != null)
        {
            RotateWeapon(); // Rotace zbran� na z�klad� pozice my�i
        }
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        RemoveWeapon(); // Odstra� starou zbra�

        // Vytvo� novou zbra� a p�ipoj ji k bodu
        currentGun = Instantiate(weaponPrefab, gunHoldPoint.position, Quaternion.identity, gunHoldPoint);
        currentWeaponName = weaponPrefab.name; // Ulo�� n�zev nov� zbran�
    }

    public void RemoveWeapon()
    {
        if (currentGun != null)
        {
            Destroy(currentGun);
            currentGun = null;
            currentWeaponName = null;
        }
    }

    private void LoadWeapon(string weaponName)
    {
        // Najde a na�te zbra� podle n�zvu
        GameObject weaponPrefab = Resources.Load<GameObject>("Weapons/" + weaponName);

        if (weaponPrefab != null)
        {
            EquipWeapon(weaponPrefab);
        }
        else
        {
            Debug.LogError("Weapon not found: " + weaponName);
        }
    }

    private void RotateWeapon()
    {
        // Z�sk�n� pozice my�i v hern�m sv�t�
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Sm�r od zbran� k my�i
        Vector3 direction = (mousePosition - currentGun.transform.position).normalized;

        // Z�sk�n� �hlu mezi zbran� a my��
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Pokud je my� vlevo od hr��e, zrcadlit zbra� (flipnout podle X-osy)
        if (mousePosition.x < transform.position.x)
        {
            currentGun.transform.localScale = new Vector3(1, 1, 1); 
        }
        else
        {
            currentGun.transform.localScale = new Vector3(1, 1, 1); 
        }

        // Rotace zbran� podle sm�ru my�i
        currentGun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
