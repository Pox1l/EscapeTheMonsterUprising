using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get; private set; }

    public float timeSpentInScene; // Èas strávený v druhé scénì
    public int rescuedNPCs; // Poèet zachránìných NPC
    public int reward; // Odmìna za záchranu

    private void Awake()
    {
        // Zajištìní, že existuje pouze jedna instance GameStats
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Udržení objektu pøi pøechodu mezi scénami
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Zvyšování èasu stráveného v aktuální scénì
        if (SceneManager.GetActiveScene().buildIndex == 1) // Zmìòte na index vaší druhé scény
        {
            timeSpentInScene += Time.deltaTime;
        }
    }
}