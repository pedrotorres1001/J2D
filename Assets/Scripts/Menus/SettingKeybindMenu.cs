using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SettingKeybindMenu : MonoBehaviour
{
    public KeybindManager keybindManager; // Reference to KeybindManager

    [Header("UI Elements")]
    [SerializeField] private Button jumpButton;
    [SerializeField] private TextMeshProUGUI jumpKeyText;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private TextMeshProUGUI moveLeftKeyText;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private TextMeshProUGUI moveRightKeyText;
    [SerializeField] private Button grapplingHookButton;
    [SerializeField] private TextMeshProUGUI grapplingHookKeyText;
    [SerializeField] private Button interactButton;
    [SerializeField] private TextMeshProUGUI interactKeyText;
    [SerializeField] private Button swingPickaxeButton;
    [SerializeField] private TextMeshProUGUI swingPickaxeKeyText;
    [SerializeField] private Button backButton;

    [Header("Menu References")]
    [SerializeField] private GameObject settingsMenuController;
    [SerializeField] private GameObject keybindMenu;

    private void OnEnable()
    {
        // Pause the game and disable player input
        Time.timeScale = 0f;
        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
            playerInput.enabled = false;

        // Update UI with current keybinds
        UpdateKeybindUI();
    }

    private void OnDisable()
    {
        // Resume the game and re-enable player input
        Time.timeScale = 1f;
        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
            playerInput.enabled = true;
    }

    private void Start()
    {
        // Add listeners for rebinding
        jumpButton.onClick.AddListener(() => StartRebinding("Jump", 0, jumpKeyText));
        moveLeftButton.onClick.AddListener(() => StartRebinding("Move", 1, moveLeftKeyText)); // Index 1 = Left
        moveRightButton.onClick.AddListener(() => StartRebinding("Move", 2, moveRightKeyText)); // Index 2 = Right
        grapplingHookButton.onClick.AddListener(() => StartRebinding("GrapplingHook", 0, grapplingHookKeyText));
        interactButton.onClick.AddListener(() => StartRebinding("Interact", 0, interactKeyText));
        swingPickaxeButton.onClick.AddListener(() => StartRebinding("SwingPickaxe", 0, swingPickaxeKeyText));
        backButton.onClick.AddListener(BackToSettingsMenu);

        // Load saved keybinds on startup
        keybindManager.LoadKeybinds();
        UpdateKeybindUI();
    }

    private void UpdateKeybindUI()
    {
        // Update displayed keybind texts
        jumpKeyText.text = GetBindingDisplayString("Jump", 0);
        moveLeftKeyText.text = GetBindingDisplayString("Move", 1);
        moveRightKeyText.text = GetBindingDisplayString("Move", 2);
        grapplingHookKeyText.text = GetBindingDisplayString("GrapplingHook", 0);
        interactKeyText.text = GetBindingDisplayString("Interact", 0);
        swingPickaxeKeyText.text = GetBindingDisplayString("SwingPickaxe", 0);
    }

    private string GetBindingDisplayString(string actionName, int bindingIndex)
    {
        var action = keybindManager.InputSystem_Actions.FindAction(actionName);
        if (action != null && bindingIndex >= 0 && bindingIndex < action.bindings.Count)
        {
            return InputControlPath.ToHumanReadableString(
                action.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );
        }
        return "Not Bound";
    }

    private void StartRebinding(string actionName, int bindingIndex, TextMeshProUGUI keyText)
    {
        keyText.text = "Press a key...";
        keybindManager.StartRebinding(actionName, bindingIndex, newBinding =>
        {
            keyText.text = InputControlPath.ToHumanReadableString(
                newBinding,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );
            keybindManager.SaveKeybinds(); // Save changes after rebinding
        });
    }

    private void BackToSettingsMenu()
    {
        keybindMenu.SetActive(false);
        settingsMenuController.SetActive(true);
    }
}
