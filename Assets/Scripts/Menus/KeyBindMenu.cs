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
            if (keyBindMenu.GetChild(i).name == "JumpButton")
            {
                keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = KeyManager.KM.jump.ToString();
                Debug.Log(KeyManager.KM.jump.ToString());
            }
            else if (keyBindMenu.GetChild(i).name == "MoveLeftButton")
            {
                keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = KeyManager.KM.moveleft.ToString();
                Debug.Log(KeyManager.KM.moveleft.ToString());
            }
            else if (keyBindMenu.GetChild(i).name == "MoveRightButton")
            {
                keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = KeyManager.KM.moveright.ToString();
                Debug.Log(KeyManager.KM.moveright.ToString());
            }
            else if (keyBindMenu.GetChild(i).name == "GrapplingHookButton")
            {
                keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = KeyManager.KM.grapplinghook.ToString();
                Debug.Log(KeyManager.KM.grapplinghook.ToString());
            }
            else if (keyBindMenu.GetChild(i).name == "InteractButton")
            {
                keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = KeyManager.KM.interact.ToString();
                Debug.Log(KeyManager.KM.interact.ToString());
            }
            else if (keyBindMenu.GetChild(i).name == "AttackButton")
            {
                keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = KeyManager.KM.attack.ToString();
                Debug.Log(KeyManager.KM.attack.ToString());
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
