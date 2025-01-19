using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRespawn : MonoBehaviour
{
    private GameObject player;
    [SerializeField] int damage;

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player");    
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player")) {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);

            player.transform.position = new Vector2(PlayerPrefs.GetFloat("RespawnX"), PlayerPrefs.GetFloat("RespawnY"));
        }
    }
}
