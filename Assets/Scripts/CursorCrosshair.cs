using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCrosshair : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouseCursorPos;

        // Zajist�, �e kurzor je v�dy nad ostatn�mi UI prvky
        Vector3 cursorPos = transform.position;
        cursorPos.z = 0; // Pokud je pou��v�n Canvas, z-index by m�l b�t na nule
        transform.position = cursorPos;
    }
}
