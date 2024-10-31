using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Attack properties
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackPoint;
    public int attackDamage;

    // Knockback properties
    [SerializeField] public float knockbackForce;
    [SerializeField] public float knockbackTime;
    private float knockbackCounter;

    // Movement properties
    [SerializeField] private float speed;
    [SerializeField] private float leftBoundary;
    [SerializeField] private float rightBoundary;
    private float moveDirection = 1;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
    }

    void FixedUpdate()
    {
        if (knockbackCounter <= 0)
        {
            rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
        }
        else
        {
            // Knockback logic for the enemy (removed)
            knockbackCounter -= Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        if (transform.position.x >= rightBoundary)
        {
            moveDirection = -1;
        }
        else if (transform.position.x <= leftBoundary)
        {
            moveDirection = 1;
        }
    }

    public void Attack()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D playerCollider in hitPlayers)
        {
            if (playerCollider.CompareTag("Player"))
            {
                Player player = playerCollider.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(attackDamage);

                    // Apply knockback to the player
                    Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                    if (playerRb != null)
                    {
                        Vector2 knockbackDirection = (player.transform.position - attackPoint.position).normalized;
                        playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Apply knockback to player upon collision
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                playerRb.velocity = knockbackDirection * knockbackForce;
            }

            // Remove knockback logic for the enemy
            // knockedFromRight = collision.transform.position.x > transform.position.x;
            // knockbackCounter = knockbackTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}