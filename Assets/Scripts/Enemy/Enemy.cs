using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;

        StartCoroutine(ColorChangeCoroutine());

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add death logic here (e.g., play animation, destroy object)
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Access the Attack method from the EnemyMovement component
            EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.Attack();
            }
            else
            {
                Debug.LogWarning("EnemyMovement component not found on Enemy!");
            }
        }
    }

    private IEnumerator ColorChangeCoroutine()
    {
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        Color damaged = Color.red;
        Color original = Color.white;

        // Change the color
        sprite.color = damaged;

        // Wait for the duration
        yield return new WaitForSeconds(0.5f);

        // Revert to the original color
        sprite.color = original;
    }
}
