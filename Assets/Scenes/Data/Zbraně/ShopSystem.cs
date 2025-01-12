using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    public GameObject shopUI; // UI panel obchodu
    public GunHolder gunHolder; // Reference na GunHolder
    public PlayerStats playerStats; // Reference na hr��ovy pen�ze
    public Weapon[] weaponsForSale; // Pole zbran�, kter� si hr�� m��e koupit

    private bool playerInRange = false;

    void Start()
    {
        // Pokud gunHolder nen� p�i�azen v inspektoru, pokus�me se z�skat instanci ze singletonu
        if (gunHolder == null)
        {
            gunHolder = GunHolder.Instance; // P�i�azen� ze singletonu
        }

        if (gunHolder == null)
        {
            Debug.LogError("GunHolder st�le nen� p�i�azen! Zkontroluj, zda je singleton spr�vn� inicializov�n.");
            return;
        }

        Debug.Log("ShopSystem: GunHolder je nyn� p�i�azen.");

        if (shopUI != null)
            shopUI.SetActive(false); // Skr�t obchod p�i startu
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShopUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (shopUI != null)
                shopUI.SetActive(false); // Zav��t obchod p�i odchodu
        }
    }

    private void ToggleShopUI()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(!shopUI.activeSelf);
        }
    }

    public void BuyWeapon(int weaponIndex)
    {
        Weapon weapon = weaponsForSale[weaponIndex];

        if (playerStats.money >= weapon.cost)
        {
            playerStats.money -= weapon.cost; // Ode��st pen�ze
            EquipWeapon(weapon); // Vybavit hr��e zbran�
            Debug.Log($"Zakoupil jsi zbra�: {weapon.weaponName}");
        }
        else
        {
            Debug.Log("Nem� dost pen�z!");
        }
    }

    private void EquipWeapon(Weapon weapon)
    {
        // Zkontroluj, zda je weaponPrefab p�i�azen�
        if (weapon.weaponPrefab == null)
        {
            Debug.LogError($"Zbra� '{weapon.weaponName}' nem� p�i�azen� weaponPrefab! P�i�a� ho v inspektoru.");
            return;
        }

        // Zkontroluj, zda je gunHolder p�i�azen�
        if (gunHolder == null)
        {
            Debug.LogError("GunHolder nen� p�i�azen!");
            return;
        }

        Debug.Log("EquipWeapon: gunHolder je p�i�azen, p�ipojuji zbra�.");

        // P�ed p�i�azen�m nov� zbran� zkontrolujeme, jestli u� n�jakou m�me
        gunHolder.DetachGun(); // Odstran� aktu�ln� zbra�, pokud existuje

        // P�ipojen� nov� zbran� na GunHolder
        gunHolder.AttachGun(weapon.weaponPrefab);
    }
}
