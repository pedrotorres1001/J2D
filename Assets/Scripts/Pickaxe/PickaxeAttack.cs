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
        // Only check for collisions with the enemy or vital parts
        if (other.CompareTag("Enemy") && isAttacking)
        {
            // Apply regular damage
            other.GetComponent<Enemy>().TakeDamage(attackDamage);
            isAttacking = false;
        }
        else if (other.CompareTag("Vital") && isAttacking)
        {
            // Apply vital damage
            Transform enemyTransform = other.transform.parent;
            if (enemyTransform != null && enemyTransform.CompareTag("Enemy"))
            {
                enemyTransform.GetComponent<Enemy>().TakeDamage(attackDamage * vitalDamageMultiplier);
                isAttacking = false;
            }
        }
    }

    public void Attack() 
    {
        isAttacking = true;
    }
}