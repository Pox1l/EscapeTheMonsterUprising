using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Hr�� se nep�esune pry� mezi sc�nami

        // Zajist�, �e nebude existovat v�ce instanc� hr��e
        if (FindObjectsOfType<PlayerManager>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
