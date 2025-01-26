using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Trigger : MonoBehaviour
{
    public bool isPlayerInArea;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = false;
        }
    }
}
