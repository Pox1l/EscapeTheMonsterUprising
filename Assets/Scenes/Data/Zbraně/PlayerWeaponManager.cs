using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance { get; private set; }

    [SerializeField] private Transform gunHoldPoint;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private SpriteRenderer debugGunSpriteRenderer; // Přidáno pro vizuální kontrolu v inspektoru

    private GameObject currentGun;
    private SpriteRenderer gunSpriteRenderer;
    private string currentWeaponName;
    private string saveFilePath;
    private List<string> purchasedWeapons;
    private bool hasWarnedGunMissing = false;


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

        if (playerSpriteRenderer == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerSpriteRenderer = player.GetComponent<SpriteRenderer>() ?? player.GetComponentInChildren<SpriteRenderer>();

                if (playerSpriteRenderer != null)
                    Debug.Log("✅ PlayerSpriteRenderer automaticky nalezen.");
                else
                    Debug.LogError("❌ PlayerSpriteRenderer se nepodařilo najít na hráči.");
            }
            else
            {
                Debug.LogError("❌ Objekt s tagem 'Player' nebyl nalezen.");
            }
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
        currentWeaponName = weaponPrefab.name;

        gunSpriteRenderer = currentGun.GetComponentInChildren<SpriteRenderer>(true);

        if (gunSpriteRenderer == null)
        {
            GameObject taggedGun = GameObject.FindGameObjectWithTag("Gun");
            if (taggedGun != null)
            {
                gunSpriteRenderer = taggedGun.GetComponent<SpriteRenderer>();
                Debug.Log("GunSpriteRenderer nalezen přes tag 'Gun'.");
            }
        }

        if (gunSpriteRenderer == null)
        {
            Debug.LogError("❌ SpriteRenderer nebyl nalezen na instanci zbraně: " + currentGun.name);
        }
        else
        {
            // Nastavení vrstvy a pořadí
            if (playerSpriteRenderer != null)
            {
                gunSpriteRenderer.sortingLayerID = playerSpriteRenderer.sortingLayerID;
                gunSpriteRenderer.sortingOrder = playerSpriteRenderer.sortingOrder + 1;
                Debug.Log($"✅ GunSpriteRenderer vrstvy zarovnán s hráčem ({SortingLayer.IDToName(gunSpriteRenderer.sortingLayerID)}), order: {gunSpriteRenderer.sortingOrder}");
            }

            debugGunSpriteRenderer = gunSpriteRenderer; // viditelné v inspektoru
        }
    }

    public void RemoveWeapon()
    {
        if (currentGun == null) return;
        Destroy(currentGun);
        currentGun = null;
        gunSpriteRenderer = null;
        debugGunSpriteRenderer = null;
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
        {
            EquipWeapon(prefab);
        }
        else
        {
            Debug.LogError("Weapon not found: " + weaponName);
        }
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
        if (gunSpriteRenderer == null)
        {
            if (!hasWarnedGunMissing)
            {
                Debug.LogWarning("⚠️ gunSpriteRenderer je null – zbraň možná ještě není instancována.");
                hasWarnedGunMissing = true;
            }
            return;
        }

        hasWarnedGunMissing = false; // resetujeme, pokud už zbraň existuje

        if (playerSpriteRenderer == null)
        {
            Debug.LogWarning("⚠️ playerSpriteRenderer není přiřazen.");
            return;
        }

        if (playerMovement.y > 0.1f)
        {
            gunSpriteRenderer.sortingOrder = playerSpriteRenderer.sortingOrder - 1;
            Debug.Log("🔽 Zbraň za hráčem");
        }
        else
        {
            gunSpriteRenderer.sortingOrder = playerSpriteRenderer.sortingOrder + 1;
            Debug.Log("🔼 Zbraň před hráčem");
        }

        Debug.Log($"🎯 [DEBUG] Player Layer: {playerSpriteRenderer.sortingLayerName} / {playerSpriteRenderer.sortingOrder} | Gun Layer: {gunSpriteRenderer.sortingLayerName} / {gunSpriteRenderer.sortingOrder}");
    }


    [System.Serializable]
    public class WeaponSaveData
    {
        public List<string> weapons;
    }
}