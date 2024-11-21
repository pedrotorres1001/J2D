using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MonoBehaviour
{

    [SerializeField] private Animator animator;
    public bool isPickaxeOnHand;


    private void Start() {
        isPickaxeOnHand = true;
    }

    void Update()
    {
        if(isPickaxeOnHand)
        {
            if (Input.GetMouseButtonDown(0))  // Left click to swing pickaxe
            {
                SwingPickaxe();
            }
        }
    }

    private void SwingPickaxe() {
        animator.SetTrigger("Swing");

        GetComponent<PickaxeBreakBlock>().BreakBlock();
    }
}
