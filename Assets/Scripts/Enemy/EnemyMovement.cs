using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    public int attackDamage;
    [SerializeField] private bool hitPlayer;
    [SerializeField] private float attackCooldown = 2f;
    private float lastAttackTime;

    // Movement properties
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private float moveDirection = 1;

    // Boundaries
    [SerializeField] private float leftBoundary;
    [SerializeField] private float rightBoundary;

    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= 10)
        {
            FollowPlayer();
        }
        else
        {
            MovementBoundaries();
        }
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // Ensure the enemy stays grounded
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        if (hit.collider == null || hit.collider.tag != "Ground")
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    public void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(gameObject.transform.position, 0.5f, playerLayer);
            foreach (Collider2D player in hitPlayers)
            {
                player.GetComponent<Player>().TakeDamage(attackDamage);
            }
            lastAttackTime = Time.time;
        }
    }

    public void MovementBoundaries()
    {
        if (transform.position.x >= rightBoundary)
        {
            moveDirection = -1;
        }
        else if (transform.position.x <= leftBoundary)
        {
            moveDirection = 1;
        }

        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
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

    private void OnDrawGizmosSelected()
    {
        if (gameObject.transform.position == null)
            return;

        Gizmos.DrawWireSphere(gameObject.transform.position, 0.5f);
    }
}