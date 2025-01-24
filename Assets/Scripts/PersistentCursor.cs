using UnityEngine;

public class PersistentCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // Textura kurzoru
    public Vector2 hotspot = Vector2.zero; // Bod kliknut� kurzoru
    public CursorMode cursorMode = CursorMode.Auto; // Re�im kurzoru

    private static bool cursorInitialized = false;

    void Awake()
    {

        // Zajist�, �e tento objekt nebude zni�en p�i zm�n� sc�ny
        DontDestroyOnLoad(gameObject);

        if (!cursorInitialized)
        {
            SetCursor();
            cursorInitialized = true;
        }
    }

    void SetCursor()
    {
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode); // Nastav� vlastn� kurzor
        Cursor.visible = true; // Zajist�, �e kurzor je viditeln�
        Cursor.lockState = CursorLockMode.None; // Ujist� se, �e kurzor nen� zamknut�
    }
}
