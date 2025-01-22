using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] GameObject startRespawnPoint1;
    [SerializeField] SaveManager saveManager;
    [SerializeField] GameObject player;
    [SerializeField] Tilemap map1;

    private int currentLevel;
    public int CurrentLevel { get; set; }

    private AudioManager audioManager;
    private Player playerScript;
    private string filePath;
    public bool firstSpawn;

    private float sessionStartTime;
    private float totalPlayTime; 
    public float TotalPlayTime
    {
        get { return totalPlayTime;  }
        set { totalPlayTime = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("Filename", "saveData.json");

        filePath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Filename"));
        saveManager.LoadData();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        // set the first respawn point


        playerScript = player.GetComponent<Player>();

        // Definir o Player no SaveManager
        saveManager.SetPlayerReference(playerScript);

        // Verificar se o arquivo de save existe
        if (!File.Exists(filePath))
        {
            currentLevel = 1;
            player.transform.position = startRespawnPoint1.transform.position;
            Debug.Log("No save file found to load.");
        }

        firstSpawn = true;

        // Iniciar o tempo de sessão
        sessionStartTime = Time.time;
    }

    private void Update()
    {
        totalPlayTime += Time.time - sessionStartTime;
        sessionStartTime = Time.time;

        SetRespawnPoints();

        // Calcular o tempo total de jogo
        /*if (!firstSpawn)
        {
            totalPlayTime = Time.time - sessionStartTime;
        }*/


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

    private void SetRespawnPoints()
    {
        if(currentLevel == 1)
        {
            PlayerPrefs.SetFloat("FirstRespawnX", startRespawnPoint1.transform.position.x);
            PlayerPrefs.SetFloat("FirstRespawnY", startRespawnPoint1.transform.position.y);
        }
        else if(currentLevel == 2)
        {

        }
    }
}
