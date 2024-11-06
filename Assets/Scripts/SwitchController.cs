using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [SerializeField] private GameObject door;


    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E)) {
            if(door != null) {
                door.GetComponent<DoorController>().OpenDoor();
            }
        }
    }
}
