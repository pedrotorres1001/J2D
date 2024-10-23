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
    private Animator animator;
    public bool isPickaxeOnHand;


    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isPickaxeOnHand = true;
    }

    void Update()
    {
        if(isPickaxeOnHand)
        {
            HandleDirection();

            if (Input.GetMouseButtonDown(0))  // Left click to swing pickaxe
            {
                SwingPickaxe();
            }

        }
    }

    private void HandleDirection() {
        if (playerMovement.direction > 0)
        {
            transform.position = rightHand.transform.position;
            attackPoint.transform.position = transform.position + new Vector3(0.4f, 0.4f);    // muda o ponto de ataque para direita
            spriteRenderer.flipX = false;
        }
        else if (playerMovement.direction < 0)
        {
            transform.position = leftHand.transform.position; 
            attackPoint.transform.position = transform.position + new Vector3(-0.4f, 0.4f);    // muda o ponto de ataque para esquerda
            spriteRenderer.flipX = true;
        }
    }

    private void SwingPickaxe() {
        animator.SetTrigger("Swing");

        GetComponent<PickaxeBreakBlock>().BreakBlock();
        GetComponent<PickaxeAttack>().Attack();
    }
}
