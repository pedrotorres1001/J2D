using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookAttack : MonoBehaviour
{
    public int attackDamage;
    public int vitalDamageMultiplier; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only check for collisions with the enemy or vital parts
        if (other.CompareTag("Enemy"))
        {
            // Apply regular damage
            other.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        else if (other.CompareTag("Vital"))
        {
            // Apply vital damage
            Transform enemyTransform = other.transform.parent;
            if (enemyTransform != null && enemyTransform.CompareTag("Enemy"))
            {
                enemyTransform.GetComponent<Enemy>().TakeDamage(attackDamage * vitalDamageMultiplier);
            }
        }
    }

}
