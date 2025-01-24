using UnityEngine;

public class PersistentCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // Textura kurzoru
    public Vector2 hotspot = Vector2.zero; // Bod kliknutí kurzoru
    public CursorMode cursorMode = CursorMode.Auto; // Režim kurzoru

    private static bool cursorInitialized = false;

    void Awake()
    {

        // Zajistí, že tento objekt nebude znièen pøi zmìnì scény
        DontDestroyOnLoad(gameObject);

        if (!cursorInitialized)
        {
            SetCursor();
            cursorInitialized = true;
        }
    }

    void SetCursor()
    {
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode); // Nastaví vlastní kurzor
        Cursor.visible = true; // Zajistí, že kurzor je viditelný
        Cursor.lockState = CursorLockMode.None; // Ujistí se, že kurzor není zamknutý
    }
}
