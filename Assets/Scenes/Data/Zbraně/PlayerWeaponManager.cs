using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance { get; private set; }

    [SerializeField] private Transform gunHoldPoint;   // Bod připojení zbraně
    private GameObject currentGun;                     // Aktuální zbraň hráče
    private string currentWeaponName;                  // Jméno aktuální zbraně

    private string saveFilePath;                       // Cesta k JSON souboru
    private List<string> purchasedWeapons;             // Seznam zakoupených zbraní

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);                 // Přetrvá mezi scénami
    }

    private void Start()
    {
        saveFilePath = Application.persistentDataPath + "/purchasedWeapons.json";
        LoadPurchasedWeapons();

        // Najdeme holder, pokud není ručně přiřazen v Inspectoru
        if (gunHoldPoint == null) gunHoldPoint = FindGunHoldPoint();

        if (!string.IsNullOrEmpty(currentWeaponName))  // načti uloženou zbraň
            LoadWeapon(currentWeaponName);
    }
    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gunHoldPoint == null || !gunHoldPoint)
            gunHoldPoint = FindGunHoldPoint();

        if (gunHoldPoint == null)
        {
            Debug.LogError("GunHoldPoint s tagem 'GunHolder' nebyl ve scéně nalezen.");
            return;
        }

        if (!string.IsNullOrEmpty(currentWeaponName))
            LoadWeapon(currentWeaponName);
    }

    private Transform FindGunHoldPoint()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("GunHolder");
        return obj != null ? obj.transform : null;
    }

    private void LoadPurchasedWeapons()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            purchasedWeapons = JsonUtility.FromJson<WeaponSaveData>(json).weapons;
        }
        else purchasedWeapons = new List<string>();
    }

    private void SavePurchasedWeapons()
    {
        var saveData = new WeaponSaveData { weapons = purchasedWeapons };
        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData, true));
    }

    /*EQUIP / REMOVE*/
    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (gunHoldPoint == null || !gunHoldPoint)
            gunHoldPoint = FindGunHoldPoint();

        if (gunHoldPoint == null)
        {
            Debug.LogError("GunHoldPoint s tagem 'GunHolder' nebyl nalezen – zbraň se nevybaví.");
            return;
        }

        RemoveWeapon();   // znič starou

        currentGun = Instantiate(weaponPrefab,
                                 gunHoldPoint.position,
                                 Quaternion.identity,
                                 gunHoldPoint);
        currentWeaponName = weaponPrefab.name;
    }

    public void RemoveWeapon()
    {
        if (currentGun == null) return;
        Destroy(currentGun);
        currentGun = null;
        currentWeaponName = null;
    }

    public bool IsWeaponEquipped(string weaponName) => currentWeaponName == weaponName;
    public void PurchaseWeapon(string weaponName)
    {
        if (purchasedWeapons.Contains(weaponName)) return;
        purchasedWeapons.Add(weaponName);
        SavePurchasedWeapons();
    }
    public bool IsWeaponPurchased(string weaponName) => purchasedWeapons.Contains(weaponName);

    private void LoadWeapon(string weaponName)
    {
        GameObject prefab = Resources.Load<GameObject>("Weapons/" + weaponName);
        if (prefab != null) EquipWeapon(prefab);
        else Debug.LogError("Weapon not found: " + weaponName);
    }

    private void Update()
    {
        if (currentGun != null) RotateWeapon();
    }

    private void RotateWeapon()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 dir = mousePos - currentGun.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        currentGun.transform.rotation = Quaternion.Euler(0, 0, angle);
        currentGun.transform.localScale =
            (angle > 90 || angle < -90) ? new Vector3(1, -1, 1) : Vector3.one;
    }
}

[System.Serializable]
public class WeaponSaveData
{
    public List<string> weapons;
}
