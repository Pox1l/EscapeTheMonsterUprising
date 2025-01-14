using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance { get; private set; }

    [SerializeField] private Transform gunHoldPoint; // Bod pøipojení zbranì
    private GameObject currentGun; // Aktuální zbraò hráèe
    private string currentWeaponName; // Jméno aktuální zbranì

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Pøetrvá mezi scénami
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(currentWeaponName))
        {
            LoadWeapon(currentWeaponName); // Naète uloženou zbraò
        }
    }

    private void Update()
    {
        if (currentGun != null)
        {
            RotateWeapon(); // Rotace zbranì na základì pozice myši
        }
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        RemoveWeapon(); // Odstraò starou zbraò

        // Vytvoø novou zbraò a pøipoj ji k bodu
        currentGun = Instantiate(weaponPrefab, gunHoldPoint.position, Quaternion.identity, gunHoldPoint);
        currentWeaponName = weaponPrefab.name; // Uloží název nové zbranì
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
        // Najde a naète zbraò podle názvu
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
        // Získání pozice myši v herním svìtì
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Smìr od zbranì k myši
        Vector3 direction = (mousePosition - currentGun.transform.position).normalized;

        // Získání úhlu mezi zbraní a myší
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Pokud je myš vlevo od hráèe, zrcadlit zbraò (flipnout podle X-osy)
        if (mousePosition.x < transform.position.x)
        {
            currentGun.transform.localScale = new Vector3(1, 1, 1); 
        }
        else
        {
            currentGun.transform.localScale = new Vector3(1, 1, 1); 
        }

        // Rotace zbranì podle smìru myši
        currentGun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
