using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.ComponentModel;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] GameObject startRespawnPoint1;
    [SerializeField] GameObject startRespawnPoint2;
    [SerializeField] GameObject startRespawnPoint3;


    [SerializeField] SaveManager saveManager;
    [SerializeField] GameObject player;
    [SerializeField] Animator irisOutAnimation;
    [SerializeField] GameObject blackPanel;
    [SerializeField] GameObject bolder1;
    [SerializeField] GameObject bolder2;


    public int currentLevel;
    private AudioManager audioManager;
    private Player playerScript;
    private string filePath;
    public bool firstSpawn;
    private float sessionStartTime;
    private float totalPlayTime;
    public float TotalPlayTime
    {
        get { return totalPlayTime; }
        set { totalPlayTime = value; }
    }



    // Start is called before the first frame update
    void Start()
    {
        blackPanel.SetActive(true);
        PlayerPrefs.SetString("Filename", "saveData.json");

        filePath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Filename"));

        // Verificar se o arquivo de save existe
        if (!File.Exists(filePath))
        {
            currentLevel = 1;
            player.transform.position = startRespawnPoint1.transform.position;
            Debug.Log("No save file found to load.");
        }
        else {
            saveManager.LoadData();

            LoadBolders();
        }

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.Play("track1");


        playerScript = player.GetComponent<Player>();

        // Definir o Player no SaveManager
        saveManager.SetPlayerReference(playerScript);

        firstSpawn = true;

        // Iniciar o tempo de sess�o
        sessionStartTime = Time.time;

        SetRespawnPoints();

        StartCoroutine(StartAnimation());
    }

    private void Update()
    {
        totalPlayTime += Time.time - sessionStartTime;
        sessionStartTime = Time.time;

    }

    private void SetRespawnPoints()
    {
        if (currentLevel == 1)
        {
            PlayerPrefs.SetFloat("FirstRespawnX", startRespawnPoint1.transform.position.x);
            PlayerPrefs.SetFloat("FirstRespawnY", startRespawnPoint1.transform.position.y);
        }
        else if (currentLevel == 2)
        {
            PlayerPrefs.SetFloat("FirstRespawnX", startRespawnPoint2.transform.position.x);
            PlayerPrefs.SetFloat("FirstRespawnY", startRespawnPoint2.transform.position.y);
        }
        else if(currentLevel == 3)
        {
            PlayerPrefs.SetFloat("FirstRespawnX", startRespawnPoint3.transform.position.x);
            PlayerPrefs.SetFloat("FirstRespawnY", startRespawnPoint3.transform.position.y);
        }
    }

    private void LoadBolders(){
        if (currentLevel == 1)
        {
            bolder1.SetActive(true);
            bolder2.SetActive(true);
        }
        else if (currentLevel == 2)
        {
            bolder1.SetActive(false);
            bolder2.SetActive(true);
        }
        else if(currentLevel == 3)
        {
            bolder1.SetActive(false);
            bolder2.SetActive(false);
        }
    }

    private IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        blackPanel.SetActive(false);
        irisOutAnimation.SetTrigger("Open");
    }

}
