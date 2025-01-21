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
                keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = GameManager.GM.jump.ToString();
                Debug.Log(GameManager.GM.jump.ToString());
            }
            else if (keyBindMenu.GetChild(i).name == "MoveLeftButton")
            {
                keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = GameManager.GM.moveleft.ToString();
                Debug.Log(GameManager.GM.moveleft.ToString());
            }
            else if (keyBindMenu.GetChild(i).name == "MoveRightButton")
            {
                keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = GameManager.GM.moveright.ToString();
                Debug.Log(GameManager.GM.moveright.ToString());
            }
            else if (keyBindMenu.GetChild(i).name == "GrapplingHookButton")
            {
                keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = GameManager.GM.grapplinghook.ToString();
                Debug.Log(GameManager.GM.grapplinghook.ToString());
            }
            else if (keyBindMenu.GetChild(i).name == "InteractButton")
            {
                keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = GameManager.GM.interact.ToString();
                Debug.Log(GameManager.GM.interact.ToString());
            }
            else if (keyBindMenu.GetChild(i).name == "AttackButton")
            {
                keyBindMenu.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = GameManager.GM.attack.ToString();
                Debug.Log(GameManager.GM.attack.ToString());
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
                GameManager.GM.jump = newKey;
                buttonText.text = GameManager.GM.jump.ToString();
                PlayerPrefs.SetString("jumpkey", GameManager.GM.jump.ToString());
                break;
            case "moveleft":
                GameManager.GM.moveleft = newKey;
                buttonText.text = GameManager.GM.moveleft.ToString();
                PlayerPrefs.SetString("moveleftkey", GameManager.GM.moveleft.ToString());
                break;
            case "moveright":
                GameManager.GM.moveright = newKey;
                buttonText.text = GameManager.GM.moveright.ToString();
                PlayerPrefs.SetString("moverightkey", GameManager.GM.moveright.ToString());
                break;
            case "grapplinghook":
                GameManager.GM.grapplinghook = newKey;
                buttonText.text = GameManager.GM.grapplinghook.ToString();
                PlayerPrefs.SetString("grapplinghookkey", GameManager.GM.grapplinghook.ToString());
                break;
            case "interact":
                GameManager.GM.interact = newKey;
                buttonText.text = GameManager.GM.interact.ToString();
                PlayerPrefs.SetString("interactkey", GameManager.GM.interact.ToString());
                break;
            case "attack":
                GameManager.GM.attack = newKey;
                buttonText.text = GameManager.GM.attack.ToString();
                PlayerPrefs.SetString("attackkey", GameManager.GM.attack.ToString());
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
