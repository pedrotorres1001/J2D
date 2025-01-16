using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] GameObject startRespawnPoint;
    private AudioManager audioManager;


    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        audioManager.Play("background");

        //gameObject.GetComponent<SceneFadeManager>().FadeIn();

        // set the first respawn point
        PlayerPrefs.SetFloat("RespawnX", startRespawnPoint.transform.position.x);
        PlayerPrefs.SetFloat("RespawnY", startRespawnPoint.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
