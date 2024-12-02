using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class SettingsMenuGame : MonoBehaviour
{
    public Slider audioSlider;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public GameObject settingsMenu;
    public GameObject pauseMenu;
    public Button backButton;

    private Resolution[] resolutions;

    void Start()
    {
        audioSlider.value = PlayerPrefs.GetFloat("AudioVolume", 1.0f);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        Resolution currentResolution = Screen.currentResolution;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionOption = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(resolutionOption);

            if (resolutions[i].width == currentResolution.width && resolutions[i].height == currentResolution.height)
            {
                resolutionDropdown.value = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);

        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }

    public void SetAudioVolume(float volume)
    {
        PlayerPrefs.SetFloat("AudioVolume", volume);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void ReturnToMainMenu()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
}