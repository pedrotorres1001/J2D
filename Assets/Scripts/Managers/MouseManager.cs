using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Texture2D cursorTexture; // Drag your arrow texture here in the Inspector
    public float cursorScale = 2f; // Scale factor for the cursor
    public float cursorDistance = 2f; // Fixed distance from the player
    private GameObject customCursor; // GameObject to represent the custom cursor
    private Camera mainCamera;
    private Transform playerTransform;

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

        // Find the player transform (assuming it has a "Player" tag)
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            customCursor.SetActive(true);

            // Get the mouse position in world space
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure it's in 2D space

            // Get the player's position
            Vector3 playerPosition = playerTransform.position;

            // Calculate the direction from the player to the mouse
            Vector3 direction = (mousePosition - playerPosition).normalized;

            // Place the custom cursor at a fixed distance from the player in that direction
            Vector3 cursorPosition = playerPosition + direction * cursorDistance;
            customCursor.transform.position = cursorPosition;

            // Rotate the cursor to face the mouse position
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            customCursor.transform.rotation = Quaternion.Euler(0, 0, angle + 90); // Adjust to align the texture properly
        }
        else
        {
            // Hide the custom cursor when the button is not pressed
            customCursor.SetActive(false);
        }
    }
}
