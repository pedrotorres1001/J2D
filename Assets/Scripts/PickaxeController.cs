using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MonoBehaviour
{
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand; 
    [SerializeField] private PlayerMovement playerMovement; 
    
    [SerializeField] private GameObject attackPoint;

    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        if (playerMovement.direction > 0)
        {
            transform.position = rightHand.transform.position;
            attackPoint.transform.position = transform.position + new Vector3(0.45f, 0.45f);    // muda o ponto de ataque para direita
            spriteRenderer.flipX = false;
        }
        else if (playerMovement.direction < 0)
        {
            transform.position = leftHand.transform.position; 
            attackPoint.transform.position = transform.position + new Vector3(-0.45f, 0.45f);    // muda o ponto de ataque para esquerda
            spriteRenderer.flipX = true;
        }
    }
}
