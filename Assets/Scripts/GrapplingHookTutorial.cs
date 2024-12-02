using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookTutorial : MonoBehaviour
{
    [SerializeField] private GameObject mouseInteraction;

    private bool playerCollided;

    private void Update() {
        if((Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.E)) && playerCollided)
        {
            mouseInteraction.SetActive(false);
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            playerCollided = true;
            mouseInteraction.SetActive(true);
        }
    }
}
