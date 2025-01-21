using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public Button loadButton;
    public Color buttonDisabledColor = Color.white; // Default text color


    private string filePath;

    private void Start()
    {

        PlayerPrefs.SetString("Filename", "saveData.json");

        filePath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Filename"));

    }

    private void Update()
    {
        if (!File.Exists(filePath))
        {
            loadButton.interactable = false;
            loadButton.GetComponent<ButtonHoverTMPColor>().enabled = false;
            loadButton.GetComponentInChildren<TMP_Text>().color = buttonDisabledColor; 
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Village");
    }

    public void QuitGame()
    {
        print("quit");
        Application.Quit();
    }
}
