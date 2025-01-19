using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenu;

    public Button loadButton;

    private string filePath;

    private void Start()
    {

        PlayerPrefs.SetString("Filename", "saveData.json");

        filePath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Filename"));


        if (!File.Exists(filePath))
        {
            loadButton.interactable = false;
            Debug.Log("No save file found to load.");
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Village");
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
