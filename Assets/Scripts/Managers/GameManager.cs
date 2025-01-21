using System.Collections;
using System.Collections.Generic;
using Unity.Services.Apis.Admin.RemoteConfig;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    public KeyCode jump { get; set; }
    public KeyCode moveleft { get; set; }
    public KeyCode moveright { get; set; }
    public KeyCode grapplinghook { get; set; }
    public KeyCode interact { get; set; }
    public KeyCode attack { get; set; }

    void Awake()
    {
        if (GM == null)
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
        else if (GM != this)
        {
            Destroy(gameObject);
        }

        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpkey", "Space"));
        moveleft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("moveleftkey", "A"));
        moveright = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("moverightkey", "D"));
        grapplinghook = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("grapplinghookkey", "Mouse1"));
        interact = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("interactkey", "F"));
        attack = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("attackkey", "Mouse0"));
    }

    void Start()
    {

    }

    void Update()
    {

    }
}