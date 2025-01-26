using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeAttack : MonoBehaviour
{
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;
    public int attackDamage;
    public int vitalDamageMultiplier; 

    private bool isAttacking;

    private void OnTriggerStay2D(Collider2D other)
    {
        switch (isAttacking)
        {
            case true:
                if (other.CompareTag("Vital"))
                {
                    // Apply vital damage
                    Transform enemyTransform = other.transform.parent;
                    if (enemyTransform != null && enemyTransform.CompareTag("Enemy"))
                    {
                        if (enemyTransform.GetComponent<Enemy>() != null)
                        {
                            enemyTransform.GetComponent<Enemy>().TakeDamage(attackDamage * vitalDamageMultiplier);
                        }
                        else if (enemyTransform.GetComponent<Boss2>() != null)
                        {
                            enemyTransform.GetComponent<Boss>().ProjectPlayer(1500); ;
                            enemyTransform.GetComponent<Boss>().TakeDamage(attackDamage * vitalDamageMultiplier);
                        }
                        else if (enemyTransform.GetComponent<Boss>() != null)
                        {
                            enemyTransform.GetComponent<Boss>().TakeDamage(attackDamage * vitalDamageMultiplier);
                        }
                        else if (enemyTransform.GetComponent<Boss3>() != null)
                        {
                            enemyTransform.GetComponent<Boss3>().TakeDamage(attackDamage * vitalDamageMultiplier);
                        }
                        isAttacking = false;
                    }
                }
                break;
            default:
                break;
        }
    }

    public void Attack() 
    {
        isAttacking = true;
    }
}