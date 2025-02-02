using TMPro;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponPrefabs; // Prefaby dostupn�ch zbran�
    [SerializeField] private int[] weaponCosts; // Ceny jednotliv�ch zbran�
    [SerializeField] private TMP_Text moneyText; // TextMeshPro pro zobrazen� aktu�ln�ch pen�z
    [SerializeField] private TMP_Text feedbackText; // TextMeshPro pro zp�tnou vazbu hr��i
    [SerializeField] private GameObject shopUI; // UI panel obchodu
    [SerializeField] private TMP_Text[] weaponButtons; // Texty tla��tek v obchod�

    private bool isShopOpen = false;
    private bool playerInShopZone = false;

    private void Start()
    {
        UpdateMoneyUI();
        UpdateShopUI();

        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Shop UI is not assigned.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInShopZone)
        {
            ToggleShopUI();
        }
    }

    public void ToggleShopUI()
    {
        if (shopUI != null)
        {
            isShopOpen = !isShopOpen;
            shopUI.SetActive(isShopOpen);
            UpdateShopUI(); // Aktualizuj tla��tka p�i otev�en� obchodu
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

        GameObject weaponPrefab = weaponPrefabs[weaponIndex];
        string weaponName = weaponPrefab.name;

        if (PlayerWeaponManager.Instance.IsWeaponPurchased(weaponName))
        {
            PlayerWeaponManager.Instance.EquipWeapon(weaponPrefab);
            ShowFeedback("Weapon equipped: " + weaponName, Color.green);
        }
        else
        {
            int weaponCost = weaponCosts[weaponIndex];
            if (PlayerMoney.Instance.HasEnoughMoney(weaponCost))
            {
                PlayerMoney.Instance.SpendMoney(weaponCost);
                PlayerWeaponManager.Instance.PurchaseWeapon(weaponName);
                PlayerWeaponManager.Instance.EquipWeapon(weaponPrefab);

                UpdateMoneyUI();
                UpdateShopUI();
                ShowFeedback("Weapon purchased: " + weaponName, Color.green);
            }
            else
            {
                ShowFeedback("Not enough money to buy this weapon.", Color.red);
            }
        }
    }

    private void UpdateShopUI()
    {
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            if (weaponButtons[i] != null)
            {
                string weaponName = weaponPrefabs[i].name;

                if (PlayerWeaponManager.Instance.IsWeaponPurchased(weaponName))
                {
                    if (PlayerWeaponManager.Instance.IsWeaponEquipped(weaponName))
                    {
                        weaponButtons[i].text = "Equipped";
                    }
                    else
                    {
                        weaponButtons[i].text = "Owned";
                    }
                }
                else
                {
                    weaponButtons[i].text = "Buy " + weaponCosts[i];
                }
            }
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
            CancelInvoke(nameof(ClearFeedback));
            Invoke(nameof(ClearFeedback), 3f);
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
            playerInShopZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInShopZone = false;
            if (isShopOpen)
            {
                ToggleShopUI();
            }
        }
    }
}
