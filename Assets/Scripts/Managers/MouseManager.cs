using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Texture2D customCursorTexture; // Drag your texture here in the Inspector
    public Vector2 hotSpot = Vector2.zero; // Set the cursor's "active point"
    public CursorMode cursorMode = CursorMode.Auto;
    void Start()
    {
        int targetSize = 32; // Desired size of the cursor
        Texture2D resizedCursor = ResizeTexture(customCursorTexture, targetSize, targetSize);
        Cursor.SetCursor(resizedCursor, hotSpot, CursorMode.Auto);
    }

    Texture2D ResizeTexture(Texture2D original, int width, int height)
    {
        Texture2D resized = new Texture2D(width, height);
        Color[] pixels = original.GetPixels(0, 0, original.width, original.height);
        resized.SetPixels(pixels);
        resized.Apply();
        return resized;
    }
}
