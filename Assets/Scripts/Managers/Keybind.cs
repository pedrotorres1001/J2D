using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Keybind : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject keybindPanel;

    private void Start()
    {
        buttonText.text = PlayerPrefs.GetString("key");  
    }

    private void Update()
    {
        if (buttonText.text == "Press a key")
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(kcode))
                {
                    buttonText.text = kcode.ToString();
                    PlayerPrefs.SetString("key", kcode.ToString());
                    PlayerPrefs.Save();
                    Time.timeScale = 1; // Resume time
                }
            }
        }
    }

    public void ChangeKey()
    {
        buttonText.text = "Press a key";
        Time.timeScale = 0; // Freeze time
    }

    public void BackButton()
    {
        settingsPanel.SetActive(true);
        keybindPanel.SetActive(false);
        Time.timeScale = 1; // Resume time
    }
}