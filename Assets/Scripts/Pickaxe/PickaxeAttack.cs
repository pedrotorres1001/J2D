using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeAttack : MonoBehaviour
{
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;
    public int attackDamage;
    public int vitalDamageMultiplier = 2; 

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D collider in hitEnemies)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<Enemy>().TakeDamage(attackDamage);
            }

            if (collider.CompareTag("Vital"))
            {
                Transform enemyTransform = collider.transform.parent;
                if (enemyTransform != null && enemyTransform.CompareTag("Enemy"))
                {
                    enemyTransform.GetComponent<Enemy>().TakeDamage(attackDamage * vitalDamageMultiplier);
                }
            }
        }
    }
}