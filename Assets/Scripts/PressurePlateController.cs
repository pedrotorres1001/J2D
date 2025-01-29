using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class PressurePlateController : MonoBehaviour
{
    public GameObject weightObject;
    public GameObject unlockableObject;
    public GameSceneManager gameSceneManager;

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject == weightObject)
        {
            unlockableObject.GetComponent<DoorController>().OpenDoor();
            Destroy(weightObject, 5);
        }
    }

}


