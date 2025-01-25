using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private Sprite switchUsed;
    private bool isColliding = false;

    

    private void Start() {
        isColliding = false;
    }

    private void Update() 
    {
        if(isColliding && Input.GetKeyDown(KeyCode.F)) 
        {
            if(door != null) 
            {

                door.GetComponent<DoorController>().OpenDoor();
                gameObject.GetComponent<SpriteRenderer>().sprite = switchUsed;
            
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
