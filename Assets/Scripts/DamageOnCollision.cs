using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    // script to damage an entity when colliding
    [SerializeField] private int damage;
    [SerializeField] private GameObject player;

    private void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.tag == "Player") {
            Player healthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            if(healthScript != null) 
            {
                healthScript.TakeDamage(damage); // change to respawn in last checkpoint
            }
        }
        else if(other.gameObject.tag == "Enemy") {
            // destroy it
        }
    }
}
