using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenu;
    public void NewGame()
    {
        SceneManager.LoadScene("Village");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        print("quit");
        Application.Quit();
    }
}
