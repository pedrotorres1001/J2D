using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    private GameObject player;

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player");    
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player")) {
            player.transform.position = new Vector2(PlayerPrefs.GetFloat("RespawnX"), PlayerPrefs.GetFloat("RespawnY"));
        }
    }
}
