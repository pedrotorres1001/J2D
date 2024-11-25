using UnityEngine;
using System.Collections;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public int attackDamage;
    [SerializeField] private float attackCooldown = 2f;
    private float lastAttackTime;

    // Movement properties
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private bool facingRight = false;

    private Transform player;

    // Dash properties
    [SerializeField] private float dashSpeed = 10f;
    private bool isDashing = false;
    private bool hasDashed = false;

    private float moveDirection = 1;

    [SerializeField] private GameObject leftBoundary;
    [SerializeField] private GameObject rightBoundary;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector3 localScale = transform.localScale;
        if (localScale.x > 0)
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        rb.velocity = new Vector2(-speed, rb.velocity.y);
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float yDifference = Mathf.Abs(transform.position.y - player.position.y);

        if (isDashing) return;

        if (distanceToPlayer <= 7 && yDifference <= 0.2f)
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
            hasDashed = false; 
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
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, 1f);
            
            foreach (Collider2D target in hitPlayers)
            {
                if (target.CompareTag("Player"))
                {
                    target.GetComponent<Player>().TakeDamage(attackDamage);
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
            if (facingRight) Flip();
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else if (transform.position.x <= leftBoundary.transform.position.x)
        {
            moveDirection = 1;
            if (!facingRight) Flip();
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    private IEnumerator PrepareAndDash()
    {
        isDashing = true;

        gameObject.GetComponent<Animator>().SetTrigger("attack");

        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(2f);

        Vector2 dashDirection = (player.position - transform.position).normalized;
        dashDirection.y = 0;
        rb.velocity = dashDirection * dashSpeed;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float timeToReachPlayer = distanceToPlayer / dashSpeed;
        yield return new WaitForSeconds(timeToReachPlayer);

        rb.velocity = Vector2.zero;
        isDashing = false;
        hasDashed = true;

        ProjectPlayer(dashDirection);
    }

    private void ProjectPlayer(Vector2 dashDirection)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 projectionForce = dashDirection * 5f; // Adjust the force as needed
            playerRb.AddForce(projectionForce, ForceMode2D.Impulse);
        }
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
        localScale.x *= -1;
        transform.localScale = localScale;
        facingRight = !facingRight;
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