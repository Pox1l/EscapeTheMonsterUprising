using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Hráè se nepøesune pryè mezi scénami

        // Zajistí, že nebude existovat více instancí hráèe
        if (FindObjectsOfType<PlayerManager>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
