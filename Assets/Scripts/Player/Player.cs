using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int health = 100;
    private Rigidbody2D rb;
    private bool isTakingDamage = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator ColorChangeCoroutine()
    {
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        Color damaged = Color.red;
        Color original = Color.white;

        sprite.color = damaged;

        yield return new WaitForSeconds(0.5f);

        sprite.color = original;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isTakingDamage)
        {
            StartCoroutine(HandleCollisionWithEnemy(collision));
        }
    }

    private IEnumerator HandleCollisionWithEnemy(Collision2D collision)
    {
        isTakingDamage = true;

        EnemyMovement enemyMovement = collision.gameObject.GetComponent<EnemyMovement>();

        if (enemyMovement != null)
        {

            TakeDamage(enemyMovement.attackDamage);

            if (rb != null)
            {
                Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;

                rb.velocity = new Vector2(knockbackDirection.x * enemyMovement.knockbackForce, rb.velocity.y);


                yield return new WaitForSeconds(0.2f);
            }
        }
        isTakingDamage = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isTakingDamage)
        {
            StartCoroutine(HandleCollisionWithEnemy(collision));
        }
    }
}