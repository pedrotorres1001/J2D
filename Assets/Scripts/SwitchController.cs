using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private Sprite switchUsed;


    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.Q)) {
            if(door != null) {
                door.GetComponent<DoorController>().OpenDoor();
                gameObject.GetComponent<SpriteRenderer>().sprite = switchUsed;
            }
        }
    }
}
