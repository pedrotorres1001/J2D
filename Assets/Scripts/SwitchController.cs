using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private Sprite switchUsed;
    private bool isColliding = false;
    private AudioManager audioManager;

    private void Start() {
        isColliding = false;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update() 
    {
        if(isColliding && Input.GetKey(KeyManager.KM.interact)) 
        {
            if(door != null) 
            {
                audioManager.Play("lever");

                door.GetComponent<DoorController>().OpenDoor();
                gameObject.GetComponent<SpriteRenderer>().sprite = switchUsed;
                this.enabled = false;
            
            }
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) {
            isColliding = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) {
            isColliding = false;
        }
    }
}
