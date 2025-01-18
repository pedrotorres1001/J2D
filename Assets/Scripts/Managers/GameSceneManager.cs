using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] GameObject startRespawnPoint;
    [SerializeField] SaveManager saveManager;
    [SerializeField] GameObject player;
    [SerializeField] Tilemap map1;

    public int currentLevel;
    public Tilemap currentMap;

    private AudioManager audioManager;
    private Player playerScript;
    private string filePath;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Filename"));
        saveManager.LoadData();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.Play("background");

        // set the first respawn point
        PlayerPrefs.SetFloat("RespawnX", startRespawnPoint.transform.position.x);
        PlayerPrefs.SetFloat("RespawnY", startRespawnPoint.transform.position.y);

        playerScript = player.GetComponent<Player>();

        // Definir o Player no SaveManager
        saveManager.SetPlayerReference(playerScript);

        // Verificar se o arquivo de save existe
        if (!File.Exists(filePath))
        { 
            currentLevel = 1;
            Debug.Log("No save file found to load.");
        }
    }

    private void Update()
    {
        if (currentLevel == 1)
        {
            // currentMap = map1;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            saveManager.SaveData(); // Salvar dados diretamente do SaveManager
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            // Verificar se o arquivo de save existe
            if (File.Exists(filePath))
            {
                saveManager.LoadData(); // Carregar dados diretamente no SaveManager
            }
        }
    }

}
