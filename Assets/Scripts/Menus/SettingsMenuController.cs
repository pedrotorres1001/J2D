using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class SettingsMenuController : MonoBehaviour
{
    public Slider audioSlider;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;

    [SerializeField] private GameObject lastMenu;

    void Start()
    {
        audioSlider.value = PlayerPrefs.GetFloat("AudioVolume", 0.5f);

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

        if(fullscreenToggle != null)
        {
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        }
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

    public void ReturnToLastMenu()
    {
        lastMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}