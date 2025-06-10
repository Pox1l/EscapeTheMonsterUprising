using TMPro;
using UnityEngine;
using UnityEngine.UI; // Potøebné pro práci s UI Button

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private GameObject interactText; // Interakèní text nebo ikona
    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private int[] weaponCosts;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private TMP_Text[] weaponButtons;
    [SerializeField] private Button[] weaponButtonObjects; // Reference na Buttony
    [SerializeField] private int healCost = 25; // Cena za heal
    [SerializeField] private int[] npcRequirements; // Poèet NPC potøebný pro odemèení zbraní

    private bool isShopOpen = false;
    private bool playerInShopZone = false;

    private void Start()
    {
        UpdateMoneyUI();
        UpdateShopUI();
        ApplyNPCUnlocks();

        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Shop UI is not assigned.");
        }
        if (interactText != null)
            interactText.SetActive(false);

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
            UpdateShopUI();
        }
        else
        {
            Debug.LogError("Shop UI is not assigned.");
        }
    }

    public void CloseShopUI()
    {
        if (shopUI != null)
        {
            isShopOpen = false;
            shopUI.SetActive(false); // Skrýt UI
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

        // Kontrola, jestli je zbraò odemèena podle poètu zachránìných NPC
        int totalNPCs = SaveSystem.LoadNPCCount();
        if (totalNPCs < npcRequirements[weaponIndex])
        {
            ShowFeedback("Weapon locked! Save more NPCs to unlock.", Color.red);
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

    public void BuyHeal()
    {
        if (PlayerHealth.Instance.CurrentHealth >= PlayerHealth.Instance.maxHealth)
        {
            ShowFeedback("You already have full HP!", Color.yellow);
            return;
        }

        if (PlayerMoney.Instance.HasEnoughMoney(healCost))
        {
            PlayerMoney.Instance.SpendMoney(healCost);
            PlayerHealth.Instance.Heal(25);

            UpdateMoneyUI();
            ShowFeedback("Healed +25 HP!", Color.green);
        }
        else
        {
            ShowFeedback("Not enough money for heal.", Color.red);
        }
    }

    private void UpdateShopUI()
    {
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            if (weaponButtons[i] != null)
            {
                string weaponName = weaponPrefabs[i].name;

                // Kontrola odemèení zbranì podle poètu NPC
                int totalNPCs = SaveSystem.LoadNPCCount();
                if (totalNPCs >= npcRequirements[i])
                {
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
                else
                {
                    // Zbraò je zamèená
                    weaponButtons[i].text = "Locked (Save " + npcRequirements[i] + " NPCs)";
                }

                // Deaktivování tlaèítka, pokud zbraò není odemèena
                if (weaponButtonObjects[i] != null)
                {
                    weaponButtonObjects[i].interactable = totalNPCs >= npcRequirements[i];
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

            if (interactText != null)
                interactText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInShopZone = false;

            if (interactText != null)
                interactText.SetActive(false);

            if (isShopOpen)
                ToggleShopUI();
        }
    }


    //Nastavení NPC Requirements(mùžeš to pøidat pøímo do inspektoru)
    public void ApplyNPCUnlocks()
    {
        npcRequirements = new int[] { 0, 5, 10, 20, 30, 40, 50}; // Nastavení poètu NPC pro odemèení zbraní
    }
}
