using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] GameObject blacksmithSpeechBubble;

    private bool isColliding;

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == blacksmithSpeechBubble)
        {
            isColliding = true;        
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == blacksmithSpeechBubble)
        {
            isColliding = false;
        }
    }
}
