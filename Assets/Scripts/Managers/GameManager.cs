using System.Collections;
using System.Collections.Generic;
using Unity.Services.Apis.Admin.RemoteConfig;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyManager : MonoBehaviour
{
    public static KeyManager KM;

    public KeyCode jump { get; set; }
    public KeyCode moveleft { get; set; }
    public KeyCode moveright { get; set; }
    public KeyCode moveup { get; set; }
    public KeyCode movedown { get; set; }
    public KeyCode grapplinghook { get; set; }
    public KeyCode interact { get; set; }
    public KeyCode attack { get; set; }
    
    void Awake()
    {
        if (KM == null)
        {
            DontDestroyOnLoad(gameObject);
            KM = this;
        }
        else if (KM != this)
        {
            Destroy(gameObject);
        }

        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpkey", "Space"));
        moveleft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("moveleftkey", "A"));
        moveright = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("moverightkey", "D"));
        moveup = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("moveupkey", "W"));
        movedown = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("movedownkey", "S"));
        grapplinghook = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("grapplinghookkey", "Mouse1"));
        interact = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("interactkey", "F"));
        attack = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("attackkey", "Mouse0"));
    }

}