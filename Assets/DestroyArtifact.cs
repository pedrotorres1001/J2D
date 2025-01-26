using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyArtifact : MonoBehaviour
{
    public GameObject objectToDestroy;  
    public GameObject player;           
    public Transform teleportLocation;  

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == objectToDestroy)
        {
            Destroy(objectToDestroy);

            if (player != null && teleportLocation != null)
            {
                player.transform.position = teleportLocation.position;
            }
        }
    }
}
