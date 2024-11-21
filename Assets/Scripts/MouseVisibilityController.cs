using UnityEngine;

public class MouseVisibilityController : MonoBehaviour
{
    public GameObject blockHighlight; // Assign the blockHighlight GameObject in the Inspector
    public float movementThreshold = 5f; // Minimum distance the mouse must move to be considered significant
    public float hideCursorDelay = 1f; // Time in seconds before hiding the cursor

    private Vector3 lastMousePosition; // To track mouse movement
    private float inactivityTimer = 0f; // Timer to track inactivity

    void Start()
    {
        // Save the initial mouse position
        lastMousePosition = Input.mousePosition;

        // Ensure the cursor starts visible
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if blockHighlight is active
        //if (blockHighlight != null && blockHighlight.activeSelf)
        //{
        //    Cursor.visible = false; // Hide the cursor if blockHighlight is active
        //    return;
        //}

        // Calculate mouse movement
        float distanceMoved = Vector3.Distance(Input.mousePosition, lastMousePosition);

        if (distanceMoved >= movementThreshold)
        {
            // Mouse moved significantly: Reset the timer and show the cursor
            inactivityTimer = 0f;
            Cursor.visible = true;
            lastMousePosition = Input.mousePosition; // Update the last mouse position
        }
        else
        {
            // Increment the inactivity timer
            inactivityTimer += Time.deltaTime;

            // Hide the cursor if inactivity time exceeds the delay
            if (inactivityTimer >= hideCursorDelay)
            {
                Cursor.visible = false;
            }
        }
    }
}