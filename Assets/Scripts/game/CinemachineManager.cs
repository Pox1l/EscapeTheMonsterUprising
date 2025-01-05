using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    private static CinemachineManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Uchovej kameru mezi sc�nami
        }
        else
        {
            Destroy(gameObject); // Zajisti, �e nebude duplik�t
        }
    }
}
