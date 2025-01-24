using UnityEngine;

public class TestCursor : MonoBehaviour
{
    void Start()
    {
        // Vytvo�en� testovac� textury o rozm�rech 32x32
        Texture2D testCursor = new Texture2D(32, 32, TextureFormat.RGBA32, false);

        // Vytvo�en� barvy (pr�hledn� barva)
        Color transparentColor = new Color(0, 0, 0, 0);

        // Vytvo�en� �erven�ho �tverce v lev�m horn�m rohu textury
        for (int y = 0; y < testCursor.height; y++)
        {
            for (int x = 0; x < testCursor.width; x++)
            {
                // �erven� �tverec v lev�m horn�m rohu
                if (x < 16 && y < 16)
                    testCursor.SetPixel(x, y, Color.red);  // Nastav� �ervenou barvu
                else
                    testCursor.SetPixel(x, y, transparentColor);  // Nastav� pr�hlednou barvu
            }
        }

        // Aplikace pixel� na texturu
        testCursor.Apply();

        // Nastaven� kurzoru
        Cursor.SetCursor(testCursor, Vector2.zero, CursorMode.Auto);

        Debug.Log("Testovac� kurzor nastaven.");
    }
}
