using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenuController : MonoBehaviour
{
    public Slider audioSlider;
    // Resolution Dropdown textmeshpro
    public Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        // Initialize Audio Slider
        audioSlider.onValueChanged.AddListener(SetVolume);
        audioSlider.value = AudioListener.volume;

        // Populate Resolution Dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        foreach (var res in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(res.width + "x" + res.height));
        }
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        // Select the current resolution
        int currentResolutionIndex = System.Array.FindIndex(resolutions, res => 
            res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // Update global volume
        Debug.Log("Volume set to: " + volume); // Log to ensure it works
    }

    public void SetResolution(int index)
    {
        Resolution selectedResolution = resolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        Debug.Log("Resolution set to: " + selectedResolution.width + "x" + selectedResolution.height);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
