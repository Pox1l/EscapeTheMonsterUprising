using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    /* ---------- UI ---------- */
    [Header("Speed UI")]
    [SerializeField] private Button buySpeedBtn;
    [SerializeField] private TextMeshProUGUI speedLevelTxt;
    [SerializeField] private TextMeshProUGUI speedCostTxt;

    [Header("Regen UI")]
    [SerializeField] private Button buyRegenBtn;
    [SerializeField] private TextMeshProUGUI regenLevelTxt;
    [SerializeField] private TextMeshProUGUI regenCostTxt;

    [Header("Common UI")]
    [SerializeField] private Button resetBtn;
    [SerializeField] private TextMeshProUGUI msgTxt;

    /* ---------- config ---------- */
    [Header("Speed Config")]
    [SerializeField] private float speedIncrement = 0.1f;
    [SerializeField] private int[] speedCosts = { 20, 40, 80, 160, 320 };

    [Header("Regen Config")]
    [SerializeField] private float regenIncrement = 1f;           // +1 HP/s každé lvl
    [SerializeField] private int[] regenCosts = { 30, 60, 120, 240 };

    /* ---------- runtime ---------- */
    private PlayerController playerController;
    private PlayerHealth playerHealth;

    private float baseSpeed;
    private int speedLvl;
    private int regenLvl;

    private const string PREF_SPEED_LVL = "SpeedUpgradeLvl";
    private const string PREF_SPEED_BASE = "BasePlayerSpeed";
    private const string PREF_REGEN_LVL = "RegenUpgradeLvl";

    /* ---------- životní cyklus ---------- */
    private void Awake()
    {
        SceneManager.sceneLoaded += (_, __) => RebindPlayer();
        RebindPlayer();
    }

    private void Start()
    {
        buySpeedBtn.onClick.AddListener(BuySpeed);
        buyRegenBtn.onClick.AddListener(BuyRegen);
        resetBtn.onClick.AddListener(ResetAll);

        RefreshUI();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= (_, __) => RebindPlayer();
    }

    /* ---------- player binding ---------- */
    private void RebindPlayer()
    {
        GameObject go = GameObject.FindWithTag("Player");
        if (!go)
        {
            playerController = null;
            playerHealth = null;
            return;
        }

        playerController = go.GetComponent<PlayerController>();
        playerHealth = go.GetComponent<PlayerHealth>();

        /* --- SPEED základ --- */
        if (PlayerPrefs.HasKey(PREF_SPEED_BASE))
            baseSpeed = PlayerPrefs.GetFloat(PREF_SPEED_BASE);
        else
        {
            baseSpeed = playerController.moveSpeed;
            PlayerPrefs.SetFloat(PREF_SPEED_BASE, baseSpeed);
        }

        speedLvl = PlayerPrefs.GetInt(PREF_SPEED_LVL, 0);
        regenLvl = PlayerPrefs.GetInt(PREF_REGEN_LVL, 0);

        ApplySpeed();
        ApplyRegen();
        RefreshUI();
    }

    /* ---------- upgrade nákup ---------- */
    private void BuySpeed()
    {
        if (!EnsurePlayer() || speedLvl >= speedCosts.Length) return;

        int cost = speedCosts[speedLvl];
        if (!Player_XP.Instance.HasEnoughXP(cost)) { Flash("Nedostatek XP!"); return; }

        Player_XP.Instance.SpendXP(cost);
        speedLvl++;
        PlayerPrefs.SetInt(PREF_SPEED_LVL, speedLvl);

        ApplySpeed();
        RefreshUI();
        Flash("+0.1 k rychlosti");
    }

    private void BuyRegen()
    {
        if (!EnsurePlayer() || regenLvl >= regenCosts.Length) return;

        int cost = regenCosts[regenLvl];
        if (!Player_XP.Instance.HasEnoughXP(cost)) { Flash("Nedostatek XP!"); return; }

        Player_XP.Instance.SpendXP(cost);
        regenLvl++;
        PlayerPrefs.SetInt(PREF_REGEN_LVL, regenLvl);

        ApplyRegen();
        RefreshUI();
        Flash($"+{regenIncrement} HP/5s");
    }

    /* ---------- reset ---------- */
    private void ResetAll()
    {
        if (!EnsurePlayer()) return;

        speedLvl = 0;
        regenLvl = 0;
        PlayerPrefs.SetInt(PREF_SPEED_LVL, speedLvl);
        PlayerPrefs.SetInt(PREF_REGEN_LVL, regenLvl);

        ApplySpeed();
        ApplyRegen();
        RefreshUI();
        Flash("Vse resetovano");
    }

    /* ---------- apply helpers ---------- */
    private void ApplySpeed()
    {
        playerController.moveSpeed = baseSpeed + speedLvl * speedIncrement;
    }

    private void ApplyRegen()
    {
        playerHealth.regenPerSec = regenLvl * regenIncrement;
    }


    /* ---------- UI helpers ---------- */
    private void RefreshUI()
    {
        /* speed */
        bool speedMax = speedLvl >= speedCosts.Length;
        speedLevelTxt.text = $"Speed lvl: {speedLvl}/{speedCosts.Length}";
        speedCostTxt.text = speedMax ? "MAX" : $"Cost: {speedCosts[speedLvl]} XP";
        buySpeedBtn.gameObject.SetActive(!speedMax);

        /* regen */
        bool regenMax = regenLvl >= regenCosts.Length;
        regenLevelTxt.text = $"Regen lvl: {regenLvl}/{regenCosts.Length}";
        regenCostTxt.text = regenMax ? "MAX" : $"Cost: {regenCosts[regenLvl]} XP";
        buyRegenBtn.gameObject.SetActive(!regenMax);
    }

    private bool EnsurePlayer()
    {
        if (playerController && playerHealth) return true;
        RebindPlayer();
        return playerController && playerHealth;
    }

    private void Flash(string txt)
    {
        if (!msgTxt) return;
        msgTxt.text = txt;
        CancelInvoke(nameof(ClearFlash));
        Invoke(nameof(ClearFlash), 1.5f);
    }
    private void ClearFlash() => msgTxt.text = "";
}
