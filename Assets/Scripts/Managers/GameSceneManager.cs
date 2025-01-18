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

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 1;

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.Play("background");

        // set the first respawn point
        PlayerPrefs.SetFloat("RespawnX", startRespawnPoint.transform.position.x);
        PlayerPrefs.SetFloat("RespawnY", startRespawnPoint.transform.position.y);

        playerScript = player.GetComponent<Player>();

        // Definir o Player no SaveManager
        saveManager.SetPlayerReference(playerScript);
    }

    private void Update()
    {
        if (currentLevel == 1)
        {
            // currentMap = map1;
        }

        // Press 'U' to save data
        if (Input.GetKeyDown(KeyCode.U))
        {
            saveManager.SaveData(); // Salvar dados diretamente do SaveManager
        }

        // Press 'Y' to load data, but only if the save file exists
        if (Input.GetKeyDown(KeyCode.Y))
        {
            // Verificar se o arquivo de save existe
            if (File.Exists(saveManager.FilePath))
            {
                saveManager.LoadData(); // Carregar dados diretamente no SaveManager
            }
            else
            {
                Debug.Log("No save file found to load.");
            }
        }
    }

}
