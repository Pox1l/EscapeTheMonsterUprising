using UnityEngine;

public class TestCursor : MonoBehaviour
{
    void Start()
    {
        // Vytvoøení testovací textury o rozmìrech 32x32
        Texture2D testCursor = new Texture2D(32, 32, TextureFormat.RGBA32, false);

        // Vytvoøení barvy (prùhledná barva)
        Color transparentColor = new Color(0, 0, 0, 0);

        // Vytvoøení èerveného ètverce v levém horním rohu textury
        for (int y = 0; y < testCursor.height; y++)
        {
            for (int x = 0; x < testCursor.width; x++)
            {
                // Èervený ètverec v levém horním rohu
                if (x < 16 && y < 16)
                    testCursor.SetPixel(x, y, Color.red);  // Nastaví èervenou barvu
                else
                    testCursor.SetPixel(x, y, transparentColor);  // Nastaví prùhlednou barvu
            }
        }

        // Aplikace pixelù na texturu
        testCursor.Apply();

        // Nastavení kurzoru
        Cursor.SetCursor(testCursor, Vector2.zero, CursorMode.Auto);

        Debug.Log("Testovací kurzor nastaven.");
    }
}
