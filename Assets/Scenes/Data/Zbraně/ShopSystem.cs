using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    public GameObject shopUI; // UI panel obchodu
    public GunHolder gunHolder; // Reference na GunHolder
    public PlayerStats playerStats; // Reference na hráèovy peníze
    public Weapon[] weaponsForSale; // Pole zbraní, které si hráè mùže koupit

    private bool playerInRange = false;

    void Start()
    {
        // Pokud gunHolder není pøiøazen v inspektoru, pokusíme se získat instanci ze singletonu
        if (gunHolder == null)
        {
            gunHolder = GunHolder.Instance; // Pøiøazení ze singletonu
        }

        if (gunHolder == null)
        {
            Debug.LogError("GunHolder stále není pøiøazen! Zkontroluj, zda je singleton správnì inicializován.");
            return;
        }

        Debug.Log("ShopSystem: GunHolder je nyní pøiøazen.");

        if (shopUI != null)
            shopUI.SetActive(false); // Skrýt obchod pøi startu
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
                shopUI.SetActive(false); // Zavøít obchod pøi odchodu
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
            playerStats.money -= weapon.cost; // Odeèíst peníze
            EquipWeapon(weapon); // Vybavit hráèe zbraní
            Debug.Log($"Zakoupil jsi zbraò: {weapon.weaponName}");
        }
        else
        {
            Debug.Log("Nemáš dost penìz!");
        }
    }

    private void EquipWeapon(Weapon weapon)
    {
        // Zkontroluj, zda je weaponPrefab pøiøazený
        if (weapon.weaponPrefab == null)
        {
            Debug.LogError($"Zbraò '{weapon.weaponName}' nemá pøiøazený weaponPrefab! Pøiøaï ho v inspektoru.");
            return;
        }

        // Zkontroluj, zda je gunHolder pøiøazený
        if (gunHolder == null)
        {
            Debug.LogError("GunHolder není pøiøazen!");
            return;
        }

        Debug.Log("EquipWeapon: gunHolder je pøiøazen, pøipojuji zbraò.");

        // Pøed pøiøazením nové zbranì zkontrolujeme, jestli už nìjakou máme
        gunHolder.DetachGun(); // Odstraní aktuální zbraò, pokud existuje

        // Pøipojení nové zbranì na GunHolder
        gunHolder.AttachGun(weapon.weaponPrefab);
    }
}
