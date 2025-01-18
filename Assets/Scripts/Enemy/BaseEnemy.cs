using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseEnemy : Enemy
{
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int attackDamage;

    private bool hasDashed = false;
    // Dash properties
    private float lastAttackTime;
    private GameObject player;
    private float distanceToPlayer;
    private Rigidbody2D rb;
    private bool isDashing;
    private EnemyPatrol enemyPatrol;
    private Animator animator;

    private bool isPerformingAction = false;
    private Vector2 playerLastKnownPosition;


    private bool isAttacking;
    private bool canSeePlayer;

    private PlayerMovement playerMovement;

    [SerializeField] ParticleSystem dustParticles;
    public float interval = 1f; // Intervalo em segundos
    private float timer = 0f; // Temporizador interno

    protected override void Start() {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        enemyPatrol = gameObject.GetComponent<EnemyPatrol>();
        animator = gameObject.GetComponent<Animator>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        isDashing = false;
        isAttacking = false;
        canSeePlayer = false;
    }

    private void Update() 
    {
        if (isPerformingAction || isAttacking) return; // Skip Update logic during special actions or attacks

        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        canSeePlayer = HasLineOfSight();

        if (canSeePlayer && distanceToPlayer < 6 && distanceToPlayer >= 2) 
        {
            FollowPlayer();
        }
        else {
            animator.SetBool("isRunning", false);
        }

        if (canSeePlayer && distanceToPlayer >= 6) 
        {
            StartCoroutine(PrepareAndDash());
        }
        else if (canSeePlayer && distanceToPlayer < 1f) 
        {
            Attack();
        }
        else if (!canSeePlayer) 
        {
            if (enemyPatrol != null) 
            {
                enemyPatrol.Patrol();
            }
        }

        timer += Time.deltaTime;

        if (gameObject.GetComponent<Rigidbody2D>().velocity.x != 0 && timer >= interval)
        {
            dustParticles.Play(); // Reproduz as partículas
            timer = 0f; // Reseta o temporizador   
        }
    }

    void FollowPlayer() 
    {
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;

        float direction = playerDirection.x > 0 ? 1 : -1; // Determine facing direction
        animator.SetFloat("Direction", direction); // Update animation direction
        animator.SetBool("isRunning", true); // Update animation direction
        rb.velocity = new Vector2(playerDirection.x * (speed * 3), 0); // Move only in X-axis
    }

    private bool HasLineOfSight() 
    {
        LayerMask playerMask = LayerMask.GetMask("Player", "Ground", "Destructable");

        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, sightRange, playerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, sightRange, playerMask);

        if (hitLeft.collider != null && hitLeft.collider.CompareTag("Player") 
        || hitRight.collider != null && hitRight.collider.CompareTag("Player")) 
        {
            return true;
        }
        else if(hitLeft.collider != null && hitLeft.collider.CompareTag("Ground") || 
            hitRight.collider != null && hitRight.collider.CompareTag("Ground")) 
        {
            return false;  // Line of sight blocked by ground
        }

        return false;
    }

    public override void Attack()
    {
        if (isAttacking || isPerformingAction) return; // Prevent multiple attacks or interruptions

        if (Time.time - lastAttackTime < attackCooldown) return; // Respect cooldown

        isAttacking = true; // Mark as attacking

        // Stop enemy movement during attack
        rb.velocity = Vector2.zero;

        // Reset attack state after the cooldown
        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown); // Wait for cooldown
        isAttacking = false; // Allow other actions
    }

    private IEnumerator PrepareAndDash() 
    {
        isPerformingAction = true;

        // Record the player's position before charging
        playerLastKnownPosition = player.transform.position;

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

        // Reset state
        isPerformingAction = false;
        hasDashed = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //StartCoroutine(WaitSeconds(0.5f));
        
        }
    }

    private IEnumerator WaitSeconds(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        Knockback();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(attackDamage);
            animator.SetTrigger("onAttack"); // Play attack animation


            Knockback();
        
        }
    }

    private void Knockback() {
            playerMovement.knockbackCounter = playerMovement.knockbackTotalTime;

            if(player.transform.position.x <= transform.position.x)
            {
                playerMovement.knockbackFromRight = true;
            }
            if(player.transform.position.x > transform.position.x)
            {
                playerMovement.knockbackFromRight = false;
            }  
    }

    private void OnDrawGizmos() {
        
        //Debug.DrawRay(transform.position, Vector2.left * sightRange, Color.red);
        //Debug.DrawRay(transform.position, Vector2.right * sightRange, Color.red);
    }
    

}
