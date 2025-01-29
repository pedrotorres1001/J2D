using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PressurePlateController : MonoBehaviour
{
    public GameObject weightObject;
    public GameObject unlockableObject;


    private void OnCollisionStay2D(Collision2D other) 
    {
        if(other.gameObject == weightObject)
        {
            unlockableObject.GetComponent<DoorController>().OpenDoor();
        }
    }

}


