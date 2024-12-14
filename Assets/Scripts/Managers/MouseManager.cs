using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Texture2D cursorTexture; // Drag your arrow texture here in the Inspector
    public float cursorScale = 2f; // Scale factor for the cursor
    private GameObject customCursor; // GameObject to represent the custom cursor
    private Camera mainCamera;

    void Start()
    {
        // Hide the system cursor
        Cursor.visible = false;

        // Create a custom cursor object
        customCursor = new GameObject("CustomCursor");
        SpriteRenderer cursorRenderer = customCursor.AddComponent<SpriteRenderer>();

        // Convert Texture2D to Sprite
        Rect textureRect = new Rect(0, 0, cursorTexture.width, cursorTexture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f); // Center of the sprite
        cursorRenderer.sprite = Sprite.Create(cursorTexture, textureRect, pivot);

        // Scale the cursor
        customCursor.transform.localScale = Vector3.one * cursorScale;

        // Set the sorting order to a high value to ensure the cursor renders in front
        cursorRenderer.sortingOrder = 100; // Adjust the value to your needs

        // Cache the main camera
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            customCursor.SetActive(true);

            // Get mouse position in world space
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Keep it in 2D

            // Position the custom cursor at the mouse position
            customCursor.transform.position = mousePosition;

            // Calculate direction from screen center to mouse position
            Vector3 screenCenter = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
            screenCenter.z = 0;

            Vector3 direction = mousePosition - screenCenter;

            // Calculate the rotation angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate the cursor
            customCursor.transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Adjust by -90 degrees to align with the texture
        }
        else
        {
            // Hide the custom cursor when the button is not pressed
            customCursor.SetActive(false);
        }
    }
}
