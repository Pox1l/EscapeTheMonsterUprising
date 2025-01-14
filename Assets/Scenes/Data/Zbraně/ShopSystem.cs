using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponPrefabs; // Prefaby dostupných zbraní
    [SerializeField] private int[] weaponCosts; // Ceny jednotlivých zbraní
    [SerializeField] private TMP_Text moneyText; // TextMeshPro pro zobrazení aktuálních penìz
    [SerializeField] private TMP_Text feedbackText; // TextMeshPro pro zpìtnou vazbu hráèi
    [SerializeField] private GameObject shopUI; // UI panel obchodu

    private bool isShopOpen = false; // Sledování stavu otevøeného obchodu
    private bool playerInShopZone = false; // Kontrola, zda je hráè v zónì obchodu

    private void Start()
    {
        UpdateMoneyUI(); // Aktualizace zobrazení penìz na zaèátku
        if (shopUI != null)
        {
            shopUI.SetActive(false); // Ujistíme se, že je obchod na zaèátku skrytý
        }
        else
        {
            Debug.LogWarning("Shop UI is not assigned.");
        }
    }

    private void Update()
    {
        // Zkontroluj stisknutí klávesy E pouze pokud je hráè v zónì obchodu
        if (Input.GetKeyDown(KeyCode.E) && playerInShopZone)
        {
            ToggleShopUI(); // Otevøe nebo zavøe obchod
        }
    }

    public void ToggleShopUI()
    {
        if (shopUI != null)
        {
            isShopOpen = !isShopOpen;
            shopUI.SetActive(isShopOpen); // Pøepínání viditelnosti obchodu
            // Místo nastavování timeScale na 0 použijeme unscaledTime pro správné ovládání UI bez zpomalení hry
            if (isShopOpen)
            {
                Time.timeScale = 1; // Keep the game running normally
                Time.fixedDeltaTime = 0.02f * Time.timeScale; // Ensure physics runs smoothly
            }
            else
            {
                Time.timeScale = 1; // Restore normal time flow when the shop is closed
            }
        }
        else
        {
            Debug.LogError("Shop UI is not assigned.");
        }
    }

    public void BuyWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weaponPrefabs.Length)
        {
            Debug.LogError("Invalid weapon index.");
            return;
        }

        int weaponCost = weaponCosts[weaponIndex];

        if (PlayerMoney.Instance.HasEnoughMoney(weaponCost))
        {
            PlayerMoney.Instance.SpendMoney(weaponCost); // Odeèti peníze
            PlayerWeaponManager.Instance.EquipWeapon(weaponPrefabs[weaponIndex]); // Pøidej zbraò hráèi

            UpdateMoneyUI(); // Aktualizace UI penìz
            ShowFeedback("Weapon purchased: " + weaponPrefabs[weaponIndex].name, Color.green); // Zobrazení zpìtné vazby
        }
        else
        {
            ShowFeedback("Not enough money to buy this weapon.", Color.red); // Zobrazení chyby
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: $" + PlayerMoney.Instance.GetMoney();
        }
        else
        {
            Debug.LogWarning("Money text UI is not assigned.");
        }
    }

    private void ShowFeedback(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
            CancelInvoke(nameof(ClearFeedback)); // Zruší pøedchozí vymazání zprávy
            Invoke(nameof(ClearFeedback), 3f); // Vymaže zprávu po 3 sekundách
        }
        else
        {
            Debug.LogWarning("Feedback text UI is not assigned.");
        }
    }

    private void ClearFeedback()
    {
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInShopZone = true; // Hráè vstoupil do zóny obchodu
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInShopZone = false; // Hráè opustil zónu obchodu
            if (isShopOpen)
            {
                ToggleShopUI(); // Zavøi obchod, pokud byl otevøen
            }
        }
    }
}
