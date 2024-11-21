using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D customCursor; // Drag your cursor texture here in the inspector
    public Vector2 hotspot = Vector2.zero; // Hotspot of the cursor

    void Start()
    {
        // Change the cursor
        Cursor.SetCursor(customCursor, hotspot, CursorMode.Auto);
    }
}
