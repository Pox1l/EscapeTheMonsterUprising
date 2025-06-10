using UnityEngine;

public class SecretWeaponGiver : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab; // Prefab zbranì
    [SerializeField] private string requiredWeaponName; // Název zbranì
    [SerializeField] private GameObject interactText; // Ikona/text "[E]"

    private bool playerInRange = false;

    private void Start()
    {
        if (interactText != null)
            interactText.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            GiveWeaponToPlayer();
        }
    }

    private void GiveWeaponToPlayer()
    {
        if (weaponPrefab == null)
        {
            Debug.LogWarning("Weapon prefab is not assigned.");
            return;
        }

        if (!PlayerWeaponManager.Instance.IsWeaponPurchased(requiredWeaponName))
        {
            PlayerWeaponManager.Instance.PurchaseWeapon(requiredWeaponName);
        }

        PlayerWeaponManager.Instance.EquipWeapon(weaponPrefab);
        Debug.Log("Player received and equipped secret weapon: " + requiredWeaponName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactText != null)
                interactText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactText != null)
                interactText.SetActive(false);
        }
    }
}
