using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTriggerLava : MonoBehaviour
{
    [SerializeField] GameObject lava;
    [SerializeField] GameObject stairs;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(stairs);
            lava.SetActive(true);
        }
    }
}
