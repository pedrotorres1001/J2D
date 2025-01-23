using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] public int attackDamage;


    private bool hasDashed = false;
    // Dash properties
    private Transform player;
    private float distanceToPlayer;
    private Rigidbody2D rb;
    private bool isDashing;
    private EnemyPatrol enemyPatrol;
    private Animator animator;
    private bool colidedWithPlayer;

    private bool isPerformingAction = false;
    private AudioManager audioManager;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource LoopSource;

    private Vector2 playerLastKnownPosition;
    private int direction;
    private float patrolSpeed = 3;
    private float runningSpeed = 4;

    private EyeCollider eyeCollider;
    private bool canSeePlayer;

    protected override void Start() {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        enemyPatrol = gameObject.GetComponent<EnemyPatrol>();
        animator = gameObject.GetComponent<Animator>();
        eyeCollider = gameObject.GetComponentInChildren<EyeCollider>();

        isDashing = false;
        direction = 1;
        colidedWithPlayer = false;
    }

    private void Update() 
    {
        if (isPerformingAction) return; // Skip Update logic during special actions

        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        canSeePlayer = eyeCollider.canSeePlayer;

        print(canSeePlayer);

        if (canSeePlayer && distanceToPlayer > 6) {
            enemyPatrol.isFollowingPlayer = true;
            StartCoroutine(PrepareAndDash());

        }
        
        if (canSeePlayer && distanceToPlayer <= 6 && !isPerformingAction && !colidedWithPlayer) {
            print("Running");
            animator.SetBool("isRunning", true);
            FollowPlayer();
        }
        else{
            animator.SetBool("isRunning", false);
        }

        if(!canSeePlayer && !colidedWithPlayer) {
            print("Patrolling");
            animator.SetBool("isPatrolling", true);
            Patrol();
        }
        else {
            animator.SetBool("isPatrolling", false);
        }

        if(colidedWithPlayer)
        {
            StartCoroutine(StopAndWait());
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
        float newDirection = playerDirection.x > 0 ? 1 : -1; // Determine the facing direction
        rb.velocity = new Vector2(runningSpeed * playerDirection.x, rb.velocity.y);


        // Flip the sprite if necessary
        if (newDirection != direction)
        {
            direction = (int)newDirection;
            Flip(direction);
        }
    }

    private void Patrol()
    {
        int layer = LayerMask.GetMask("Ground", "Destructable");
        
        // Check for obstacles in the current direction
        Vector2 rayDirection = direction > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 2, layer);

        if (hit.collider != null)
        {
            // Reverse direction if an obstacle is detected
            direction *= -1;
            Flip(direction);
        }

        // Apply movement based on the current direction
        rb.velocity = new Vector2(patrolSpeed * direction, rb.velocity.y);

    }

    private IEnumerator PrepareAndDash() 
    {
        isPerformingAction = true;

        // Record the player's position before charging
        playerLastKnownPosition = player.position;

        // Calculate dash direction (X-axis only)
        Vector2 dashDirection = new Vector2(playerLastKnownPosition.x - transform.position.x, 0).normalized;

        float direction = dashDirection.x > 0 ? 1 : -1; // Determine facing direction

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

    public IEnumerator StopAndWait() {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(2);
        colidedWithPlayer = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            colidedWithPlayer = true;
        }
    }


    private void Flip(float movementDirection)
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

}