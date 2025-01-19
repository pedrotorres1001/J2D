using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class KeybindManager : MonoBehaviour
{
    public InputActionAsset InputSystem_Actions; // Reference to the Input Action Asset
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    // Start rebinding process for a specific action and binding index
    public void StartRebinding(string actionName, int bindingIndex, Action<string> onComplete)
    {
        var action = InputSystem_Actions.FindAction(actionName);
        if (action == null)
        {
            Debug.LogError($"Action '{actionName}' not found.");
            return;
        }

        // Ensure binding index is valid
        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count)
        {
            Debug.LogError($"Invalid binding index {bindingIndex} for action '{actionName}'.");
            return;
        }

        // Start rebinding operation
        rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse") // Exclude mouse if needed
            .OnMatchWaitForAnother(0.1f) // Small delay to avoid accidental inputs
            .OnComplete(operation =>
            {
                string newBinding = action.bindings[bindingIndex].effectivePath;
                onComplete?.Invoke(newBinding); // Invoke callback with new binding
                rebindingOperation.Dispose();
                rebindingOperation = null; // Clean up
            })
            .Start();
    }

    // Cancel the rebinding operation if needed
    public void CancelRebinding()
    {
        if (rebindingOperation != null)
        {
            rebindingOperation.Cancel();
            rebindingOperation.Dispose();
            rebindingOperation = null;
        }
    }

    // Save keybind overrides to PlayerPrefs
    public void SaveKeybinds()
    {
        foreach (var action in InputSystem_Actions)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (!string.IsNullOrEmpty(action.bindings[i].overridePath))
                {
                    PlayerPrefs.SetString($"{action.name}_binding_{i}", action.bindings[i].overridePath);
                }
            }
        }
        PlayerPrefs.Save();
    }

    // Load keybind overrides from PlayerPrefs
    public void LoadKeybinds()
    {
        foreach (var action in InputSystem_Actions)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                string savedBinding = PlayerPrefs.GetString($"{action.name}_binding_{i}", null);
                if (!string.IsNullOrEmpty(savedBinding))
                {
                    action.ApplyBindingOverride(i, savedBinding);
                }
            }
        }
    }
}
