using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    public bool hasKey = false;
    // pøidej sem další promìnné jako peníze, XP, atd.

    void Awake()
    {
        // Singleton pattern + DontDestroy
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // aby nebyly dvì instance
        }
    }
}

