using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manti : Enemy
{
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] public int attackDamage;

    [SerializeField] ParticleSystem dustParticles;
    public float interval = 1f; // Intervalo em segundos
    private float timer = 0f; // Temporizador interno


    private bool hasDashed = false;
    // Dash properties
    private Transform player;
    private float distanceToPlayer;
    private Rigidbody2D rb;
    private bool isDashing;
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
            StartCoroutine(PrepareAndDash());
        }
        
        if (canSeePlayer && distanceToPlayer <= 6 && !isPerformingAction && !colidedWithPlayer) {
            print("Running");
            animator.SetBool("isDashing", true);
            FollowPlayer();
        }
        else{
            animator.SetBool("isDashing", false);
        }

        if(!canSeePlayer && !colidedWithPlayer) {
            print("Patrolling");
            animator.SetBool("isWalking", true);
            Patrol();
        }
        else {
            animator.SetBool("isWalking", false);
        }

        if(colidedWithPlayer)
        {
            StartCoroutine(StopAndWait());
        }

        if (gameObject.GetComponent<Rigidbody2D>().velocity.x != 0 && timer >= interval)
        {
            dustParticles.Play(); // Reproduz as part�culas
            timer = 0f; // Reseta o temporizador   
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
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1); // Wait before charging
        animator.SetBool("isDashing", true);
    
        // Gradual acceleration variables
        float elapsedTime = 0f;
        float chargeAccelerationTime = 0.1f; // Time it takes to reach max speed
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
    
        animator.SetBool("isDashing", false);
    
        // Stop and wait after charging
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1); // Wait before flipping
        Flip(direction);
    
        // Reset state
        isPerformingAction = false;
        hasDashed = true;
    
        // Check if the player is close for grabbing attack
        if (distanceToPlayer <= 2) 
        {
            StartCoroutine(GrabAttack());
        } 
        else 
        {
            StartCoroutine(PrepareAndDash());
        }
    }
    
    public IEnumerator GrabAttack() 
    {
        isPerformingAction = true;
        animator.SetBool("isGrabbing", true);
        yield return new WaitForSeconds(0.5f); // Time for grabbing animation
    
        animator.SetBool("isGrabbing", false);
        animator.SetBool("isGrabAttacking", true);
        yield return new WaitForSeconds(0.5f); // Time for grab attack animation
    
        animator.SetBool("isGrabAttacking", false);
        isPerformingAction = false;
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