using UnityEngine;
using GameAnalyticsSDK;


public class AnalyticsManager : MonoBehaviour
{
    private void Start()
    {
        // Initialize Game Analytics
        GameAnalytics.Initialize();

        // Optionally, you can log an event to check that it's working
        GameAnalytics.NewDesignEvent("game_started");
    }
}