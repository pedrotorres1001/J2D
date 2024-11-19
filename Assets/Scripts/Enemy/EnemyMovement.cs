using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public int attackDamage;
    [SerializeField] private float attackCooldown = 2f;
    private float lastAttackTime;

    // Movement properties
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private bool facingRight = false; // Start facing left

    private Transform player;

    // Dash properties
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    private bool isDashing = false;
    private bool hasDashed = false;

    private float moveDirection = 1;

    [SerializeField] private GameObject leftBoundary;
    [SerializeField] private GameObject rightBoundary;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Ensure the enemy starts facing left
        Vector3 localScale = transform.localScale;
        if (localScale.x > 0)
        {
            localScale.x *= -1; // Ensure left-facing orientation
            transform.localScale = localScale;
        }

        rb.velocity = new Vector2(-speed, rb.velocity.y);

    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isDashing) return;

        if (distanceToPlayer <= 7)
        {
            float directionX = player.position.x - transform.position.x;
            FlipTowardsPlayer(directionX);

            if (distanceToPlayer <= 5 && !hasDashed)
            {
                StartCoroutine(PrepareAndDash());
            }
            else if (distanceToPlayer > 5 || hasDashed)
            {
                FollowPlayer();
            }
        }
        else
        {
            hasDashed = false; // Reset dash if player moves out of range
            MovementBoundaries();
        }
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        FlipTowardsPlayer(direction.x);
    }

    public void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, 0.5f, playerLayer);
            foreach (Collider2D target in hitPlayers)
            {
                if (target.CompareTag("Player"))
                {
                    target.GetComponent<Player>().TakeDamage(attackDamage);
                }
                else if (target.CompareTag("Vital"))
                {
                    target.GetComponent<Player>().TakeDamage(attackDamage * 2);
                }
            }
            lastAttackTime = Time.time;
        }
    }

    public void MovementBoundaries()
    {        
        if (transform.position.x >= rightBoundary.transform.position.x)
        {
            moveDirection = -1;
            if (facingRight) Flip(); // Flip to face left
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else if (transform.position.x <= leftBoundary.transform.position.x)
        {
            moveDirection = 1;
            if (!facingRight) Flip(); // Flip to face right
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    private IEnumerator PrepareAndDash()
    {
        isDashing = true;

        // Stop for a moment
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f); // Pause before dashing

        // Perform the dash
        Vector2 dashDirection = (player.position - transform.position).normalized;
        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero; // Stop after dash
        isDashing = false;
        hasDashed = true; // Dash only once until the player moves out of range
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();
        }
    }
    
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Flip horizontally
        transform.localScale = localScale;
        facingRight = !facingRight; // Toggle the facing direction
    }
    
    private void FlipTowardsPlayer(float directionX)
    {
        if ((directionX > 0 && !facingRight) || (directionX < 0 && facingRight))
        {
            Flip();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (transform == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
