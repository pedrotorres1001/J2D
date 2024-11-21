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

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Vital"))
            {
                Transform enemyTransform = other.transform.parent;
                if (enemyTransform != null && enemyTransform.CompareTag("Enemy"))
                {
                    enemyTransform.GetComponent<Enemy>().TakeDamage(attackDamage * vitalDamageMultiplier);
                }
            }

            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
    }

    /*
    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    */
}