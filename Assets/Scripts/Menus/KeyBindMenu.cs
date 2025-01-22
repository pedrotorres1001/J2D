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
        }
    }

    void OnGUI()
    {
        keyEvent = Event.current;
        if (keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode;
            waitingForKey = false;
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

        switch (keyName)
        {
            case "jump":
                KeyManager.KM.jump = newKey;
                buttonText.text = KeyManager.KM.jump.ToString();
                PlayerPrefs.SetString("jumpkey", KeyManager.KM.jump.ToString());
                break;
            case "moveleft":
                KeyManager.KM.moveleft = newKey;
                buttonText.text = KeyManager.KM.moveleft.ToString();
                PlayerPrefs.SetString("moveleftkey", KeyManager.KM.moveleft.ToString());
                break;
            case "moveright":
                KeyManager.KM.moveright = newKey;
                buttonText.text = KeyManager.KM.moveright.ToString();
                PlayerPrefs.SetString("moverightkey", KeyManager.KM.moveright.ToString());
                break;
            case "grapplinghook":
                KeyManager.KM.grapplinghook = newKey;
                buttonText.text = KeyManager.KM.grapplinghook.ToString();
                PlayerPrefs.SetString("grapplinghookkey", KeyManager.KM.grapplinghook.ToString());
                break;
            case "interact":
                KeyManager.KM.interact = newKey;
                buttonText.text = KeyManager.KM.interact.ToString();
                PlayerPrefs.SetString("interactkey", KeyManager.KM.interact.ToString());
                break;
            case "attack":
                KeyManager.KM.attack = newKey;
                buttonText.text = KeyManager.KM.attack.ToString();
                PlayerPrefs.SetString("attackkey", KeyManager.KM.attack.ToString());
                break;
        }
        yield return null;
    }

    public void BackButton()
    {
        keyBindMenu.gameObject.SetActive(false);
        settingsMenuObject.SetActive(true);
    }
}