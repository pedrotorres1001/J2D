using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    public GameObject gameEndMenu;
    public GameObject boss;
    public Button returnToMainMenuButton;

    void Start()
    {
        if (boss != null)
        {
            gameEndMenu.SetActive(false);
        }

        if (boss == null)
        {
            gameEndMenu.SetActive(true);
        }

        if (returnToMainMenuButton != null)
        {
            returnToMainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
