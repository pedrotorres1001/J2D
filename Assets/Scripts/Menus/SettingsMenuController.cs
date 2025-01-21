using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuController : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public AudioManager audioManager;
    public Slider volumeSlider;
    public Button KeybindButton;
    public GameObject pauseMenu;
    public GameObject settingsKeybindMenu;
    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        if (audioManager != null)
        {
            // Carrega as configurações do AudioManager antes de definir o volume do slider
            audioManager.loadSettings();

            // Define o valor inicial do slider com base no volume salvo no PlayerPrefs ou um valor padrão
            volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", audioManager.musicVolume);
        }

        // Adiciona um listener ao slider para controlar o volume
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Define os valores iniciais dos dropdowns e do toggle
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen ? 1 : 0) == 1;

        // Adiciona listeners para os eventos de mudança dos dropdowns e do toggle
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        // Assign the OpenKeybindMenu method to the KeybindButton
        KeybindButton.onClick.AddListener(OpenKeybindMenu);
    }

    public void SetVolume(float volume)
    {
        if (audioManager != null)
        {
            // Ajusta o volume da música através do AudioManager
            audioManager.musicVolume = volume;

            // Salva o volume definido pelo jogador utilizando PlayerPrefs
            PlayerPrefs.SetFloat("MusicVolume", volume);

            // Força a gravação no PlayerPrefs
            PlayerPrefs.Save();
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];

        // Verifica se a resolução atual é diferente da resolução salva no PlayerPrefs
        int savedWidth = PlayerPrefs.GetInt("ResolutionWidth", Screen.currentResolution.width);
        int savedHeight = PlayerPrefs.GetInt("ResolutionHeight", Screen.currentResolution.height);

        if (resolution.width != savedWidth || resolution.height != savedHeight)
        {
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            // Salva a resolução definida pelo jogador utilizando PlayerPrefs
            PlayerPrefs.SetInt("ResolutionWidth", resolution.width);
            PlayerPrefs.SetInt("ResolutionHeight", resolution.height);
            PlayerPrefs.Save();

            // Atualiza o valor do índice da resolução selecionada no dropdown
            resolutionDropdown.value = resolutionIndex;
            Debug.Log("Resolution Width: " + PlayerPrefs.GetInt("ResolutionWidth"));
            Debug.Log("Resolution Height: " + PlayerPrefs.GetInt("ResolutionHeight"));
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        int savedFullscreen = PlayerPrefs.GetInt("Fullscreen", -1);
        int fullscreenValue = isFullscreen ? 1 : 0;

        if (fullscreenValue != savedFullscreen)
        {
            if (isFullscreen) Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            else Screen.fullScreenMode = FullScreenMode.Windowed;

            // Salva o modo de tela cheia definido pelo jogador utilizando PlayerPrefs
            PlayerPrefs.SetInt("Fullscreen", fullscreenValue);
            PlayerPrefs.Save();

            // Atualiza o valor selecionado no toggle de tela cheia
            fullscreenToggle.isOn = isFullscreen;
            Debug.Log("Fullscreen Value: " + PlayerPrefs.GetInt("Fullscreen"));
        }
    }

    public void OpenKeybindMenu()
    {
        settingsKeybindMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    
}