using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MonoBehaviour
{
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand; 
    [SerializeField] private PlayerMovement playerMovement; 

    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        if (playerMovement.direction > 0)
        {
            transform.position = rightHand.transform.position;
            spriteRenderer.flipX = false;
        }
        else if (playerMovement.direction < 0)
        {
            transform.position = leftHand.transform.position; 
            spriteRenderer.flipX = true;
        }
    }
}
