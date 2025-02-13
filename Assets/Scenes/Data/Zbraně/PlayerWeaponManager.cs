using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance { get; private set; }

    [SerializeField] private Transform gunHoldPoint; // Bod p�ipojen� zbran�
    private GameObject currentGun; // Aktu�ln� zbra� hr��e
    private string currentWeaponName; // Jm�no aktu�ln� zbran�

    private string saveFilePath; // Cesta k JSON souboru
    private List<string> purchasedWeapons; // Seznam zakoupen�ch zbran�

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
        saveFilePath = Application.persistentDataPath + "/purchasedWeapons.json";
        LoadPurchasedWeapons();

        if (!string.IsNullOrEmpty(currentWeaponName))
        {
            LoadWeapon(currentWeaponName); // Na�te ulo�enou zbra�
        }
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
        WeaponSaveData saveData = new WeaponSaveData { weapons = purchasedWeapons };
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        RemoveWeapon(); // Odstra� starou zbra�

        // Vytvo� novou zbra� a p�ipoj ji k bodu
        currentGun = Instantiate(weaponPrefab, gunHoldPoint.position, Quaternion.identity, gunHoldPoint);
        currentWeaponName = weaponPrefab.name; // Ulo�� n�zev nov� zbran�
    }

    public bool IsWeaponEquipped(string weaponName)
    {
        return currentWeaponName == weaponName;
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

    public void PurchaseWeapon(string weaponName)
    {
        if (!purchasedWeapons.Contains(weaponName))
        {
            purchasedWeapons.Add(weaponName);
            SavePurchasedWeapons();
        }
    }

    public bool IsWeaponPurchased(string weaponName)
    {
        return purchasedWeapons.Contains(weaponName);
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
    private void Update()
    {
        if (currentGun != null)
        {
            RotateWeapon(); // Rotace zbran� na z�klad� pozice my�i
        }
    }
    private void RotateWeapon()
{
    // Z�sk�n� pozice my�i v hern�m sv�t�
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePosition.z = 0f; // Ujist�me se, �e sou�adnice Z je 0

    // Sm�r od zbran� k my�i
    Vector3 direction = mousePosition - currentGun.transform.position;

    // V�po�et �hlu v radi�nech a konverze na stupn�
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    // Rotace zbran� podle sm�ru my�i
    currentGun.transform.rotation = Quaternion.Euler(0, 0, angle);

    // Pokud je �hel v�t�� ne� 90� nebo men�� ne� -90�, p�eklop�me zbra�
    if (angle > 90 || angle < -90)
    {
        currentGun.transform.localScale = new Vector3(1, -1, 1); // Flip Y
    }
    else
    {
        currentGun.transform.localScale = new Vector3(1, 1, 1); // Norm�ln� sm�r
    }
}
}

[System.Serializable]
public class WeaponSaveData
{
    public List<string> weapons;
}
