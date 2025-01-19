using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get; private set; }

    public float timeSpentInScene; // �as str�ven� v druh� sc�n�
    public int rescuedNPCs; // Po�et zachr�n�n�ch NPC
    public int reward; // Odm�na za z�chranu

    private void Awake()
    {
        // Zaji�t�n�, �e existuje pouze jedna instance GameStats
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Udr�en� objektu p�i p�echodu mezi sc�nami
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Zvy�ov�n� �asu str�ven�ho v aktu�ln� sc�n�
        if (SceneManager.GetActiveScene().buildIndex == 1) // Zm��te na index va�� druh� sc�ny
        {
            timeSpentInScene += Time.deltaTime;
        }
    }
}