using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

public class PlayerActionTracker : MonoBehaviour
{
    private string logFilePath = "Assets/Logs/game_log.txt";  // Path to the log file
    private bool hasMoved = false;        // Track if the player has moved
    private bool hasBrokenBlock = false;
    private bool hasUsedGrapplingHook = false;  // Track if the player has used the grappling hook

    [SerializeField] private GameObject check1;
    [SerializeField] private GameObject check2;
    [SerializeField] private GameObject check3;
    [SerializeField] private GameObject check4;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject enemy;
    
    void Update()
    {
        // Check if the player has moved
        CheckPlayerMovement();

        // Check if the player has broken a block
        if(hasMoved)
            CheckBlockBreaking();

        // Check if the player has used the grappling hook
        if(hasBrokenBlock && hasMoved)
            CheckGrapplingHookUse();

        if(enemy == null) {
           check4.SetActive(true); 
        }
    }

    // Check if the player has moved
    private void CheckPlayerMovement()
    {
        if (playerMovement.direction != 0)
        {
            hasMoved = true;
            LogAction("Player Moved");
            check1.SetActive(true);
        }
    }

    // Check if the player has broken a block (based on Tilemap)
    private void CheckBlockBreaking()
    {

            // If the tile has been removed (i.e., the tile is now null)
            if (PlayerPrefs.GetInt("BlockBroken") == 1)
            {
                hasBrokenBlock = true;
                LogAction("Player Broken a Block");
                check2.SetActive(true);
            }
        
    }

    // Check if the player used the grappling hook (e.g., right-click or specific key)
    private void CheckGrapplingHookUse()
    {
        if (!hasUsedGrapplingHook && Input.GetMouseButtonDown(1))  // Example: right-click for grappling hook
        {
            hasUsedGrapplingHook = true;
            LogAction("Player Used Grappling Hook");
            check3.SetActive(true);
        }
    }

    // Log the action to the log file
    private void LogAction(string action)
    {
        // Ensure the folder exists
        string logDirectory = Path.GetDirectoryName(logFilePath);
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        // Open the log file for writing (create it if it doesn't exist)
        using (StreamWriter logWriter = new StreamWriter(logFilePath, append: true))
        {
            logWriter.WriteLine("Action: " + action + " - Time: " + System.DateTime.Now);
            logWriter.WriteLine();  // Empty line for separation between actions
        }
    }
}