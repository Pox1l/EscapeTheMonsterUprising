using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponPrefabs; // Prefaby dostupn�ch zbran�
    [SerializeField] private int[] weaponCosts; // Ceny jednotliv�ch zbran�
    [SerializeField] private TMP_Text moneyText; // TextMeshPro pro zobrazen� aktu�ln�ch pen�z
    [SerializeField] private TMP_Text feedbackText; // TextMeshPro pro zp�tnou vazbu hr��i
    [SerializeField] private GameObject shopUI; // UI panel obchodu

    private bool isShopOpen = false; // Sledov�n� stavu otev�en�ho obchodu
    private bool playerInShopZone = false; // Kontrola, zda je hr�� v z�n� obchodu

    private void Start()
    {
        UpdateMoneyUI(); // Aktualizace zobrazen� pen�z na za��tku
        if (shopUI != null)
        {
            shopUI.SetActive(false); // Ujist�me se, �e je obchod na za��tku skryt�
        }
        else
        {
            Debug.LogWarning("Shop UI is not assigned.");
        }
    }

    private void Update()
    {
        // Zkontroluj stisknut� kl�vesy E pouze pokud je hr�� v z�n� obchodu
        if (Input.GetKeyDown(KeyCode.E) && playerInShopZone)
        {
            ToggleShopUI(); // Otev�e nebo zav�e obchod
        }
    }

    public void ToggleShopUI()
    {
        if (shopUI != null)
        {
            isShopOpen = !isShopOpen;
            shopUI.SetActive(isShopOpen); // P�ep�n�n� viditelnosti obchodu
            // M�sto nastavov�n� timeScale na 0 pou�ijeme unscaledTime pro spr�vn� ovl�d�n� UI bez zpomalen� hry
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
            PlayerMoney.Instance.SpendMoney(weaponCost); // Ode�ti pen�ze
            PlayerWeaponManager.Instance.EquipWeapon(weaponPrefabs[weaponIndex]); // P�idej zbra� hr��i

            UpdateMoneyUI(); // Aktualizace UI pen�z
            ShowFeedback("Weapon purchased: " + weaponPrefabs[weaponIndex].name, Color.green); // Zobrazen� zp�tn� vazby
        }
        else
        {
            ShowFeedback("Not enough money to buy this weapon.", Color.red); // Zobrazen� chyby
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
            CancelInvoke(nameof(ClearFeedback)); // Zru�� p�edchoz� vymaz�n� zpr�vy
            Invoke(nameof(ClearFeedback), 3f); // Vyma�e zpr�vu po 3 sekund�ch
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
            playerInShopZone = true; // Hr�� vstoupil do z�ny obchodu
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInShopZone = false; // Hr�� opustil z�nu obchodu
            if (isShopOpen)
            {
                ToggleShopUI(); // Zav�i obchod, pokud byl otev�en
            }
        }
    }
}
