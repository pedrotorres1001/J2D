using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    public static string lastSceneName;

    public static void LoadSettingsMenu()
    {
        lastSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("SettingsMenu");
    }

    public static void ReturnToLastScene()
    {
        if (!string.IsNullOrEmpty(lastSceneName))
        {
            SceneManager.LoadScene(lastSceneName);
        }
    }
}
