using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance { get; private set; }

    [SerializeField] private Transform gunHoldPoint;
    [SerializeField] private SpriteRenderer playerSpriteRenderer; // přiřaď v inspektoru

    private GameObject currentGun;
    private SpriteRenderer gunSpriteRenderer;
    private string currentWeaponName;
    private string saveFilePath;
    private List<string> purchasedWeapons;

    private Vector2 playerMovement;

    private readonly HashSet<int> scenesWithGunHolder = new HashSet<int> { 1, 2 };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        saveFilePath = Application.persistentDataPath + "/purchasedWeapons.json";
        LoadPurchasedWeapons();

        if (IsSceneWithGunHolder())
        {
            gunHoldPoint = FindGunHoldPoint();
            if (!string.IsNullOrEmpty(currentWeaponName))
                LoadWeapon(currentWeaponName);
        }
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!IsSceneWithGunHolder()) return;

        gunHoldPoint = FindGunHoldPoint();

        if (gunHoldPoint == null)
        {
            Debug.LogError("GunHoldPoint s tagem 'GunHolder' nebyl ve scéně nalezen.");
            return;
        }

        if (!string.IsNullOrEmpty(currentWeaponName))
            LoadWeapon(currentWeaponName);
    }

    private bool IsSceneWithGunHolder()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        return scenesWithGunHolder.Contains(currentSceneIndex);
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
        else
        {
            purchasedWeapons = new List<string>();
        }
    }

    private void SavePurchasedWeapons()
    {
        var saveData = new WeaponSaveData { weapons = purchasedWeapons };
        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData, true));
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (!IsSceneWithGunHolder()) return;

        if (gunHoldPoint == null)
            gunHoldPoint = FindGunHoldPoint();

        if (gunHoldPoint == null)
        {
            Debug.LogError("GunHoldPoint s tagem 'GunHolder' nebyl nalezen – zbraň se nevybaví.");
            return;
        }

        RemoveWeapon();

        currentGun = Instantiate(weaponPrefab, gunHoldPoint.position, Quaternion.identity, gunHoldPoint);
        gunSpriteRenderer = currentGun.GetComponent<SpriteRenderer>();
        currentWeaponName = weaponPrefab.name;
    }

    public void RemoveWeapon()
    {
        if (currentGun == null) return;
        Destroy(currentGun);
        currentGun = null;
        gunSpriteRenderer = null;
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
        if (!IsSceneWithGunHolder()) return;

        GameObject prefab = Resources.Load<GameObject>("Weapons/" + weaponName);
        if (prefab != null)
            EquipWeapon(prefab);
        else
            Debug.LogError("Weapon not found: " + weaponName);
    }

    private void Update()
    {
        if (currentGun != null) RotateWeapon();
        UpdateWeaponSorting();
    }

    private void RotateWeapon()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 dir = mousePos - currentGun.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        currentGun.transform.rotation = Quaternion.Euler(0, 0, angle);
        currentGun.transform.localScale = (angle > 90 || angle < -90) ? new Vector3(1, -1, 1) : Vector3.one;
    }

    public void SetPlayerMovement(Vector2 movement)
    {
        playerMovement = movement;
    }

    private void UpdateWeaponSorting()
    {
        if (gunSpriteRenderer == null || playerSpriteRenderer == null) return;

        if (playerMovement.y > 0.1f)
        {
            // hráč jde nahoru → zbraň za hráčem
            gunSpriteRenderer.sortingOrder = playerSpriteRenderer.sortingOrder - 1;
        }
        else
        {
            // dolů nebo do stran → zbraň před hráčem
            gunSpriteRenderer.sortingOrder = playerSpriteRenderer.sortingOrder + 1;
        }
    }
}

[System.Serializable]
public class WeaponSaveData
{
    public List<string> weapons;
}
