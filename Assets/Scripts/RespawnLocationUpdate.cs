using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnLocationUpdate : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("RespawnX", gameObject.transform.position.x);
            PlayerPrefs.SetFloat("RespawnY", gameObject.transform.position.y);
        }
    }
}
