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
                // Only check for collisions with the enemy or vital parts
                if (other.CompareTag("Enemy"))
                {
                    // Apply regular damage
                    if (other.GetComponent<Enemy>() != null)
                    {
                        other.GetComponent<Enemy>().TakeDamage(attackDamage);
                    }
                    else if (other.transform.parent.GetComponent<BossMovement>() != null)
                    {
                        other.transform.parent.GetComponent<BossMovement>().TakeDamage(attackDamage);
                    }
                    isAttacking = false;
                }
                else if (other.CompareTag("Vital"))
                {
                    // Apply vital damage
                    Transform enemyTransform = other.transform.parent;
                    if (enemyTransform != null && enemyTransform.CompareTag("Enemy"))
                    {
                        if (enemyTransform.GetComponent<Enemy>() != null)
                        {
                            enemyTransform.GetComponent<Enemy>().TakeDamage(attackDamage * vitalDamageMultiplier);
                        }
                        else if (enemyTransform.GetComponent<BossMovement>() != null)
                        {
                            enemyTransform.GetComponent<BossMovement>().TakeDamage(attackDamage * vitalDamageMultiplier);
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