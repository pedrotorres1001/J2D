using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AttackTrigger : MonoBehaviour
{
    [SerializeField] private Boss2 boss;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.inAttackRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.inAttackRange = false;
        }
    }
}
