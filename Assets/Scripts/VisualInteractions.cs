using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualInteractions : MonoBehaviour
{
    [SerializeField] private GameObject playerPrompt;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) {
            playerPrompt.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) {
            playerPrompt.SetActive(false);
        }
    }
}
