using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MonoBehaviour
{

    [SerializeField] private Animator animator;
    public bool isPickaxeOnHand;
    AudioManager audioManager;

    private void Start() {
        isPickaxeOnHand = true;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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

        audioManager.PlaySFX(audioManager.swing);
        GetComponent<PickaxeBreakBlock>().BreakBlock();
        GetComponent<PickaxeAttack>().Attack();
    }
}
