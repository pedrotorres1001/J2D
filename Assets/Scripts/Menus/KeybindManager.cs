using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeybindManager : MonoBehaviour
{
    public InputActionAsset InputSystem_Actions; // Referência ao asset de ações de entrada
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    // Inicia o processo de redefinição de tecla
    public void StartRebinding(string actionName, int bindingIndex, System.Action<string> onComplete)
    {
        var action = InputSystem_Actions.FindAction(actionName);
        if (action == null)
        {
            Debug.LogError($"Ação '{actionName}' não encontrada.");
            return;
        }

        rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse") // Excluir o mouse, se necessário
            .OnMatchWaitForAnother(0.1f) // Pequeno atraso para evitar capturas acidentais
            .OnComplete(operation =>
            {
                string newBinding = action.bindings[bindingIndex].effectivePath;
                onComplete?.Invoke(newBinding);
                rebindingOperation.Dispose();
            })
            .Start();
    }

    // Cancela o processo de redefinição
    public void CancelRebinding()
    {
        rebindingOperation?.Cancel();
        rebindingOperation?.Dispose();
    }

    // Salva os bindings no PlayerPrefs
    public void SaveKeybinds()
    {
        foreach (var action in InputSystem_Actions)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                PlayerPrefs.SetString($"{action.name}_binding_{i}", action.bindings[i].effectivePath);
            }
        }
        PlayerPrefs.Save();
    }

    // Carrega os bindings do PlayerPrefs
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
