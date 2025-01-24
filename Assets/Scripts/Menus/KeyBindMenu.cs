using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyBindMenu : MonoBehaviour
{
    Transform keyBindMenu;
    Event keyEvent;
    TextMeshProUGUI buttonText;
    KeyCode newKey;
    bool waitingForKey;

    public GameObject keyBindMenuObject;
    public GameObject settingsMenuObject;

    void Start()
    {
        keyBindMenu = transform.Find("KeybindMenu");
        keyBindMenu.gameObject.SetActive(false);
        waitingForKey = false;

        for (int i = 0; i < keyBindMenu.childCount; i++)
        {
            var button = keyBindMenu.GetChild(i).GetComponent<UnityEngine.UI.Button>();
            var buttonText = keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();

            if (keyBindMenu.GetChild(i).name == "JumpButton")
            {
                buttonText.text = KeyManager.KM.jump.ToString();
                button.onClick.AddListener(() => { SendText(buttonText); StartAssignment("jump"); });
            }
            else if (keyBindMenu.GetChild(i).name == "MoveLeftButton")
            {
                buttonText.text = KeyManager.KM.moveleft.ToString();
                button.onClick.AddListener(() => { SendText(buttonText); StartAssignment("moveleft"); });
            }
            else if (keyBindMenu.GetChild(i).name == "MoveRightButton")
            {
                buttonText.text = KeyManager.KM.moveright.ToString();
                button.onClick.AddListener(() => { SendText(buttonText); StartAssignment("moveright"); });
            }
            else if(keyBindMenu.GetChild(i).name == "MoveUpButton")
            {
                buttonText.text = KeyManager.KM.moveup.ToString();
                button.onClick.AddListener(() => { SendText(buttonText); StartAssignment("moveup"); });
            }
            else if (keyBindMenu.GetChild(i).name == "MoveDownButton")
            {
                buttonText.text = KeyManager.KM.movedown.ToString();
                button.onClick.AddListener(() => { SendText(buttonText); StartAssignment("movedown"); });
            }
            else if (keyBindMenu.GetChild(i).name == "GrapplingHookButton")
            {
                buttonText.text = KeyManager.KM.grapplinghook.ToString();
                button.onClick.AddListener(() => { SendText(buttonText); StartAssignment("grapplinghook"); });
            }
            else if (keyBindMenu.GetChild(i).name == "InteractButton")
            {
                buttonText.text = KeyManager.KM.interact.ToString();
                button.onClick.AddListener(() => { SendText(buttonText); StartAssignment("interact"); });
            }
            else if (keyBindMenu.GetChild(i).name == "AttackButton")
            {
                buttonText.text = KeyManager.KM.attack.ToString();
                button.onClick.AddListener(() => { SendText(buttonText); StartAssignment("attack"); });
            }
            else if (keyBindMenu.GetChild(i).name == "ResetButton")
            {
                button.onClick.AddListener(ResetKeyBindings);
            }
        }
    }

    void OnGUI()
    {
        keyEvent = Event.current;
        if (waitingForKey)
        {
            if (keyEvent.isKey)
            {
                newKey = keyEvent.keyCode;
                waitingForKey = false;
            }
            else if (keyEvent.isMouse)
            {
                newKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Mouse" + keyEvent.button);
                waitingForKey = false;
            }
        }
    }

    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
        {
            StartCoroutine(AssignKey(keyName));
        }
    }

    public void SendText(TextMeshProUGUI text)
    {
        buttonText = text;
    }

    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
        {
            yield return null;
        }
    }

    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;
    
        yield return WaitForKey();
    
        // Check if the new key is already assigned to another action
        if (newKey == KeyManager.KM.jump)
        {
            KeyManager.KM.jump = KeyCode.None;
            PlayerPrefs.SetString("jumpkey", KeyManager.KM.jump.ToString());
            UpdateButtonText("JumpButton", KeyManager.KM.jump.ToString());
        }
        if (newKey == KeyManager.KM.moveleft)
        {
            KeyManager.KM.moveleft = KeyCode.None;
            PlayerPrefs.SetString("moveleftkey", KeyManager.KM.moveleft.ToString());
            // check if the new key is a mouse button
            if (newKey.ToString().Contains("Mouse"))
            {
                buttonText.text = "Mouse" + keyEvent.button;
            }
            else
            {
                UpdateButtonText("MoveLeftButton", KeyManager.KM.moveleft.ToString());
            }
        }
        if (newKey == KeyManager.KM.moveright)
        {
            KeyManager.KM.moveright = KeyCode.None;
            PlayerPrefs.SetString("moverightkey", KeyManager.KM.moveright.ToString());
            UpdateButtonText("MoveRightButton", KeyManager.KM.moveright.ToString());
        }
        if (newKey == KeyManager.KM.moveup)
        {
            KeyManager.KM.moveup = KeyCode.None;
            PlayerPrefs.SetString("moveupkey", KeyManager.KM.moveup.ToString());
            UpdateButtonText("MoveUpButton", KeyManager.KM.moveup.ToString());
        }
        if (newKey == KeyManager.KM.movedown)
        {
            KeyManager.KM.movedown = KeyCode.None;
            PlayerPrefs.SetString("movedownkey", KeyManager.KM.movedown.ToString());
            UpdateButtonText("MoveDownButton", KeyManager.KM.movedown.ToString());
        }
        if (newKey == KeyManager.KM.grapplinghook)
        {
            KeyManager.KM.grapplinghook = KeyCode.None;
            PlayerPrefs.SetString("grapplinghookkey", KeyManager.KM.grapplinghook.ToString());
            UpdateButtonText("GrapplingHookButton", KeyManager.KM.grapplinghook.ToString());
        }
        if (newKey == KeyManager.KM.interact)
        {
            KeyManager.KM.interact = KeyCode.None;
            PlayerPrefs.SetString("interactkey", KeyManager.KM.interact.ToString());
            UpdateButtonText("InteractButton", KeyManager.KM.interact.ToString());
        }
        if (newKey == KeyManager.KM.attack)
        {
            KeyManager.KM.attack = KeyCode.None;
            PlayerPrefs.SetString("attackkey", KeyManager.KM.attack.ToString());
            UpdateButtonText("AttackButton", KeyManager.KM.attack.ToString());
        }
    
        // Assign the new key and update the button text immediately
        switch (keyName)
        {
            case "jump":
                KeyManager.KM.jump = newKey;
                PlayerPrefs.SetString("jumpkey", KeyManager.KM.jump.ToString());
                UpdateButtonText("JumpButton", newKey.ToString());
                break;
            case "moveleft":
                KeyManager.KM.moveleft = newKey;
                PlayerPrefs.SetString("moveleftkey", KeyManager.KM.moveleft.ToString());
                UpdateButtonText("MoveLeftButton", newKey.ToString());
                break;
            case "moveright":
                KeyManager.KM.moveright = newKey;
                PlayerPrefs.SetString("moverightkey", KeyManager.KM.moveright.ToString());
                UpdateButtonText("MoveRightButton", newKey.ToString());
                break;
            case "moveup":
                KeyManager.KM.moveup = newKey;
                PlayerPrefs.SetString("moveupkey", KeyManager.KM.moveup.ToString());
                UpdateButtonText("MoveUpButton", newKey.ToString());
                break;
            case "movedown":
                KeyManager.KM.movedown = newKey;
                PlayerPrefs.SetString("movedownkey", KeyManager.KM.movedown.ToString());
                UpdateButtonText("MoveDownButton", newKey.ToString());
                break;
            case "grapplinghook":
                KeyManager.KM.grapplinghook = newKey;
                PlayerPrefs.SetString("grapplinghookkey", KeyManager.KM.grapplinghook.ToString());
                UpdateButtonText("GrapplingHookButton", newKey.ToString());
                break;
            case "interact":
                KeyManager.KM.interact = newKey;
                PlayerPrefs.SetString("interactkey", KeyManager.KM.interact.ToString());
                UpdateButtonText("InteractButton", newKey.ToString());
                break;
            case "attack":
                KeyManager.KM.attack = newKey;
                PlayerPrefs.SetString("attackkey", KeyManager.KM.attack.ToString());
                UpdateButtonText("AttackButton", newKey.ToString());
                break;
        }
        yield return null;
    }

    void UpdateButtonText(string buttonName, string text)
    {
        var button = keyBindMenu.Find(buttonName);
        if (button != null)
        {
            var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = text;
            }
        }
    }

    void ResetKeyBindings()
    {
        KeyManager.KM.jump = KeyCode.Space;
        KeyManager.KM.moveleft = KeyCode.A;
        KeyManager.KM.moveright = KeyCode.D;
        KeyManager.KM.moveup = KeyCode.W;
        KeyManager.KM.movedown = KeyCode.S;
        KeyManager.KM.grapplinghook = KeyCode.Mouse1;
        KeyManager.KM.interact = KeyCode.F;
        KeyManager.KM.attack = KeyCode.Mouse0;

        PlayerPrefs.SetString("jumpkey", KeyManager.KM.jump.ToString());
        PlayerPrefs.SetString("moveleftkey", KeyManager.KM.moveleft.ToString());
        PlayerPrefs.SetString("moverightkey", KeyManager.KM.moveright.ToString());
        PlayerPrefs.SetString("moveupkey", KeyManager.KM.moveup.ToString());
        PlayerPrefs.SetString("movedownkey", KeyManager.KM.movedown.ToString());
        PlayerPrefs.SetString("grapplinghookkey", KeyManager.KM.grapplinghook.ToString());
        PlayerPrefs.SetString("interactkey", KeyManager.KM.interact.ToString());
        PlayerPrefs.SetString("attackkey", KeyManager.KM.attack.ToString());

        UpdateButtonText("JumpButton", KeyManager.KM.jump.ToString());
        UpdateButtonText("MoveLeftButton", KeyManager.KM.moveleft.ToString());
        UpdateButtonText("MoveRightButton", KeyManager.KM.moveright.ToString());
        UpdateButtonText("MoveUpButton", KeyManager.KM.moveup.ToString());
        UpdateButtonText("MoveDownButton", KeyManager.KM.movedown.ToString());
        UpdateButtonText("GrapplingHookButton", KeyManager.KM.grapplinghook.ToString());
        UpdateButtonText("InteractButton", KeyManager.KM.interact.ToString());
        UpdateButtonText("AttackButton", KeyManager.KM.attack.ToString());
    }

    public void BackButton()
    {
        keyBindMenu.gameObject.SetActive(false);
        settingsMenuObject.SetActive(true);
    }
}