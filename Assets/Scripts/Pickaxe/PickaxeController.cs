using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public bool isPickaxeOnHand;
    private AudioManager audioManager;

    public float attackSpeed;
    private float lastAttackTime;

    private void Start() {
        isPickaxeOnHand = true;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        lastAttackTime = -attackSpeed;  // Permite o primeiro ataque imediato
    }

    void Update()
    {
        if(isPickaxeOnHand)
        {
            // Checka se o botão do rato está pressionado e se o tempo de ataque passou
            if (Input.GetKey(KeyManager.KM.attack) && Time.time >= lastAttackTime + attackSpeed) 
            {
                SwingPickaxe();
            }
        }
    }

    private void SwingPickaxe() {
        animator.SetTrigger("Swing");

        audioManager.Play("swing");
        GetComponent<PickaxeBreakBlock>().BreakBlock();
        GetComponent<PickaxeAttack>().Attack();

        lastAttackTime = Time.time; // Atualiza o tempo do último ataque
    }
}