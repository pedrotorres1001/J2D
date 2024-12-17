using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Texture2D customCursorTexture; // Drag your texture here in the Inspector
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        if (customCursorTexture != null)
        {
            // Ensure the cursor uses the original resolution but aligns the hotspot correctly
            Vector2 hotSpot = new Vector2(customCursorTexture.width / 2, customCursorTexture.height / 2);

            // Set the custom cursor
            Cursor.SetCursor(customCursorTexture, hotSpot, cursorMode);
        }
    }
}