using UnityEngine;
using System.IO;

public class GameLogger : MonoBehaviour
{
    private string logFilePath = "Assets/Logs/game_log.txt";  // Path to the log file
    private float gameStartTime;     // Time when the game started

    void Start()
    {
        // Record the game start time (when the game begins)
        gameStartTime = Time.time;
    }

    // Write the game run time when the player quits
    void OnApplicationQuit()
    {
        float gameTime = Time.time - gameStartTime;  // Calculate the elapsed time since the game started

        // Ensure the folder exists
        string logDirectory = Path.GetDirectoryName(logFilePath);
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        // Open the log file for writing (create it if it doesn't exist)
        using (StreamWriter logWriter = new StreamWriter(logFilePath, append: true))
        {
            logWriter.WriteLine("Game Ended - " + System.DateTime.Now);
            logWriter.WriteLine("Total Game Time: " + Mathf.FloorToInt(gameTime) + " seconds");
            logWriter.WriteLine();  // Empty line for separation between sessions
        }
    }
}