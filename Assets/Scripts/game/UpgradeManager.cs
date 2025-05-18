using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    /* ---------- UI reference fields (přetáhni v Inspectoru) ---------- */
    [Header("UI Refs")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI msgText;

    [Header("Config")]
    [SerializeField] private float speedIncrement = 0.1f;          // kolik přidá každý lvl
    [SerializeField] private int[] costs = { 20, 40, 80, 160, 320 }; // cena lvl 0‑4

    /* ---------- runtime ---------- */
    private PlayerController playerController;

    private float baseSpeed;                        // NEMĚNNÁ původní rychlost
    private int currentLvl;                       // 0‑5
    private const string PREF_LVL = "SpeedUpgradeLvl";
    private const string PREF_SPEED = "BasePlayerSpeed";

    /* ---------------------------------------------------------------- */
    private void Awake()
    {
        SceneManager.sceneLoaded += (_, __) => RebindPlayer();
        RebindPlayer();                              // pokus hned při startu
    }

    private void Start()
    {
        buyButton.onClick.AddListener(Buy);
        resetButton.onClick.AddListener(ResetUpgrades);
        RefreshUI();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= (_, __) => RebindPlayer();
    }

    /* ---------- vyhledání / znovupřiřazení hráče ---------- */
    private void RebindPlayer()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        playerController = playerGO ? playerGO.GetComponent<PlayerController>() : null;

        if (!playerController)
        {
            Debug.LogWarning("UpgradeManager: PlayerController nebyl nalezen.");
            return;
        }

        // Základní rychlost načteme z PlayerPrefs, případně uložíme poprvé
        if (PlayerPrefs.HasKey(PREF_SPEED))
            baseSpeed = PlayerPrefs.GetFloat(PREF_SPEED);
        else
        {
            baseSpeed = playerController.moveSpeed;   // rychlost z prefab‑u
            PlayerPrefs.SetFloat(PREF_SPEED, baseSpeed);
        }

        currentLvl = PlayerPrefs.GetInt(PREF_LVL, 0);
        ApplySpeed();
        RefreshUI();
    }

    /* ---------- upgrade ---------- */
    private void Buy()
    {
        if (!EnsurePlayer()) return;
        if (currentLvl >= costs.Length) return;  // už max

        int price = costs[currentLvl];
        if (!Player_XP.Instance.HasEnoughXP(price))
        {
            Flash("Nedostatek XP!");
            return;
        }

        Player_XP.Instance.SpendXP(price);
        currentLvl++;
        PlayerPrefs.SetInt(PREF_LVL, currentLvl);

        ApplySpeed();
        RefreshUI();
        Flash("+0.1 speed");
    }

    /* ---------- reset ---------- */
    private void ResetUpgrades()
    {
        if (!EnsurePlayer()) return;

        currentLvl = 0;
        PlayerPrefs.SetInt(PREF_LVL, currentLvl);

        ApplySpeed();
        RefreshUI();
        Flash("Upgrady resetovany");
    }

    /* ---------- společné pomocné funkce ---------- */
    private void ApplySpeed()
    {
        playerController.moveSpeed = baseSpeed + currentLvl * speedIncrement;
    }

    private bool EnsurePlayer()
    {
        if (playerController) return true;
        RebindPlayer();
        return playerController != null;
    }

    private void RefreshUI()
    {
        bool maxed = currentLvl >= costs.Length;

        if (levelText) levelText.text = $"Speed lvl: {currentLvl}/{costs.Length}";
        if (costText) costText.text = maxed ? "MAX" : $"Cost: {costs[currentLvl]} XP";
        if (buyButton) buyButton.gameObject.SetActive(!maxed);
    }

    private void Flash(string text)
    {
        if (!msgText) return;
        msgText.text = text;
        CancelInvoke(nameof(ClearFlash));
        Invoke(nameof(ClearFlash), 1.5f);
    }
    private void ClearFlash() => msgText.text = "";
}
