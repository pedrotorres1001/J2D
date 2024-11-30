using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : Enemy
{
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] public int attackDamage;


    private bool hasDashed = false;
    // Dash properties
    private float lastAttackTime;
    private Transform player;
    private float distanceToPlayer;
    private Rigidbody2D rb;
    private bool isDashing;
    private EnemyPatrol enemyPatrol;
    private Animator animator;

    private bool isPerformingAction = false;
    private Vector2 playerLastKnownPosition;


    protected override void Start() {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        enemyPatrol = gameObject.GetComponent<EnemyPatrol>();
        animator = gameObject.GetComponent<Animator>();

        isDashing = false;
    }

    private void Update() 
    {
        if (isPerformingAction) return; // Skip Update logic during special actions

        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        bool canSeePlayer = HasLineOfSight();

        if (canSeePlayer && distanceToPlayer > 6) {
            enemyPatrol.isFollowingPlayer = true;
            FollowPlayer();
        }
        else if (canSeePlayer && distanceToPlayer <= 6) {
            enemyPatrol.isFollowingPlayer = true;
            StartCoroutine(PrepareAndDash());
        }
        else if(!canSeePlayer) {
            enemyPatrol.isFollowingPlayer = false;
        }
    }

    public void AttackPlayer() 
    {
        if (distanceToPlayer <= 5 && !hasDashed) {
            StartCoroutine(PrepareAndDash());
        }
        else if (distanceToPlayer > 5 || hasDashed) {
            FollowPlayer();
        }
    }

    void FollowPlayer() 
    {
        Vector2 playerDirection = (player.position - transform.position).normalized;

        float direction = playerDirection.x > 0 ? 1 : -1; // Determine facing direction
        animator.SetFloat("Direction", direction); // Update animation direction
        rb.velocity = new Vector2(playerDirection.x * speed, 0); // Move only in X-axis
    }

    private bool HasLineOfSight() 
    {

        int layerMask = LayerMask.GetMask("Player");

        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 6, layerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 6, layerMask);

        if (hitLeft.collider != null && hitLeft.collider.CompareTag("Player") || hitRight.collider != null && hitRight.collider.CompareTag("Player")) 
        {
            // Line of sight is clear to the player
            return true;
        }

        // Line of sight is blocked
        return false;
    }



    public override void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, 1f);
            
            foreach (Collider2D target in hitPlayers)
            {
                if (target.CompareTag("Player"))
                {
                    rb.velocity = Vector2.zero;
                    target.GetComponent<Player>().TakeDamage(attackDamage);
                }
            }
            lastAttackTime = Time.time;
        }
    }

    private IEnumerator PrepareAndDash() 
    {
        isPerformingAction = true;

        // Record the player's position before charging
        playerLastKnownPosition = player.position;

        // Calculate dash direction (X-axis only)
        Vector2 dashDirection = new Vector2(playerLastKnownPosition.x - transform.position.x, 0).normalized;

        float direction = dashDirection.x > 0 ? 1 : -1; // Determine facing direction
        animator.SetFloat("Direction", direction); // Update animation direction

        // Stop and wait before charging
        animator.SetBool("isPreparing", true);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1); // Wait before charging
        animator.SetBool("isPreparing", false);
        animator.SetBool("isCharging", true);

        // Gradual acceleration variables
        float elapsedTime = 0f;
        float chargeAccelerationTime = 1f; // Time it takes to reach max speed
        float currentSpeed = 0f;

        // Gradually increase speed
        while (elapsedTime < chargeAccelerationTime) 
        {
            elapsedTime += Time.deltaTime;
            currentSpeed = Mathf.Lerp(0, dashSpeed, elapsedTime / chargeAccelerationTime); // Gradually interpolate speed
            rb.velocity = dashDirection * currentSpeed;
            yield return null;
        }

        // Maintain max speed for the remaining dash duration
        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration - chargeAccelerationTime);

        animator.SetBool("isCharging", false);

        // Stop and wait after charging
        rb.velocity = Vector2.zero;
        animator.SetBool("isIdle", true);
        yield return new WaitForSeconds(2);
        animator.SetBool("isIdle", false);

        // Reset state
        isPerformingAction = false;
        hasDashed = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Attack();
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Attack();
        }
    }

}
