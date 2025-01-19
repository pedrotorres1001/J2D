using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SettingKeybindMenu : MonoBehaviour
{
    public KeybindManager keybindManager; // Referência ao KeybindManager
    [SerializeField] private Button jumpButton; // Botão para redefinir a tecla de pular
    [SerializeField] private TextMeshProUGUI jumpKeyText; // Texto que mostra a tecla atual de pular
    [SerializeField] private Button moveLeftButton; // Botão para redefinir a tecla de movimento para a esquerda
    [SerializeField] private TextMeshProUGUI moveLeftKeyText; // Texto que mostra a tecla atual de movimento para a esquerda
    [SerializeField] private Button moveRightButton; // Botão para redefinir a tecla de movimento para a direita
    [SerializeField] private TextMeshProUGUI moveRightKeyText; // Texto que mostra a tecla atual de movimento para a direita
    [SerializeField] private Button grapplingHookButton; // Botão para redefinir a tecla do gancho
    [SerializeField] private TextMeshProUGUI grapplingHookKeyText; // Texto que mostra a tecla atual do gancho
    [SerializeField] private Button interactButton; // Botão para redefinir a tecla de interagir
    [SerializeField] private TextMeshProUGUI interactKeyText; // Texto que mostra a tecla atual de interagir
    [SerializeField] private Button swingPickaxeButton; // Botão para redefinir a tecla de balançar a picareta
    [SerializeField] private TextMeshProUGUI swingPickaxeKeyText; // Texto que mostra a tecla atual de balançar a picareta
    [SerializeField] private Button backButton; // Botão para salvar as configurações
    [SerializeField] private GameObject settingsMenuController; // Controlador do menu de configurações
    [SerializeField] private GameObject keybindMenu; // Controlador do menu de teclas

    private void OnEnable()
    {
        // Pause the game when the keybind menu is displayed
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        // Unpause the game when the keybind menu is hidden
        Time.timeScale = 1f;
    }

    private void Start()
    {
        // Inicializa o texto do botão com o binding atual
        jumpKeyText.text = GetBindingDisplayString("Jump");
        moveLeftKeyText.text = GetBindingDisplayString("MoveLeft");
        moveRightKeyText.text = GetBindingDisplayString("MoveRight");
        grapplingHookKeyText.text = GetBindingDisplayString("GrapplingHook");
        interactKeyText.text = GetBindingDisplayString("Interact");
        swingPickaxeKeyText.text = GetBindingDisplayString("SwingPickaxe");

        // Configura o evento do botão para redefinir a tecla
        jumpButton.onClick.AddListener(() => StartRebinding("Jump", jumpKeyText));
        moveLeftButton.onClick.AddListener(() => StartRebinding("MoveLeft", moveLeftKeyText));
        moveRightButton.onClick.AddListener(() => StartRebinding("MoveRight", moveRightKeyText));
        grapplingHookButton.onClick.AddListener(() => StartRebinding("GrapplingHook", grapplingHookKeyText));
        interactButton.onClick.AddListener(() => StartRebinding("Interact", interactKeyText));
        swingPickaxeButton.onClick.AddListener(() => StartRebinding("SwingPickaxe", swingPickaxeKeyText));
        backButton.onClick.AddListener(BackToSettingsMenu); // Assign the BackToSettingsMenu method to the backButton

        // Carrega os bindings salvos ao iniciar
        keybindManager.LoadKeybinds();
    }

    private string GetBindingDisplayString(string actionName)
    {
        var action = keybindManager.InputSystem_Actions.FindAction(actionName);
        if (action != null && action.bindings.Count > 0)
        {
            return InputControlPath.ToHumanReadableString(action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        return "Not Bound";
    }

    private void StartRebinding(string actionName, TextMeshProUGUI keyText)
    {
        keyText.text = $"Press a key to rebind {actionName}...";
        keybindManager.StartRebinding(actionName, 0, newBinding =>
        {
            keyText.text = InputControlPath.ToHumanReadableString(newBinding, InputControlPath.HumanReadableStringOptions.OmitDevice);
            keybindManager.SaveKeybinds();
        });
    }

    public void BackToSettingsMenu()
    {
        settingsMenuController.SetActive(true);
        keybindMenu.SetActive(false);
    }
}