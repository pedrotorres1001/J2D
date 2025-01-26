using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manti : Enemy
{
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] public int attackDamage;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float knockbackForce = 10f;

    [SerializeField] ParticleSystem dustParticles;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource LoopSource;

    private float lastDashTime;
    private float lastAttackTime;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private PolygonCollider2D polygonCollider;

    private EyeCollider eyeCollider;
    private bool canSeePlayer;
    private float distanceToPlayer;

    private bool isDashing;
    private bool dashHitPlayer;

    private string state = "patrol";
    private float stateTimer;

    private int direction = 1;
    private float patrolSpeed = 3f;

    private int grabHits;
    private const int maxGrabHits = 3;

    private int playerHits;
    private const int maxPlayerHits = 3;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        eyeCollider = GetComponentInChildren<EyeCollider>();
        lastDashTime = -dashCooldown; // Allow immediate dash at start
        lastAttackTime = -attackCooldown; // Allow immediate attack at start

        // Ensure the polygon collider is set as a trigger
        polygonCollider.isTrigger = true;
    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        canSeePlayer = eyeCollider.canSeePlayer;
        stateTimer += Time.deltaTime;

        switch (state)
        {
            case "patrol":
                Patrol();
                if (canSeePlayer && Time.time - lastDashTime >= dashCooldown)
                {
                    state = "dash";
                    stateTimer = 0f;
                }
                break;

            case "dash":
                if (!isDashing) StartDash();
                if (stateTimer >= dashDuration)
                {
                    EndDash();
                    state = dashHitPlayer ? "stun" : "grab";
                    stateTimer = 0f;
                }
                break;

            case "grab":
                GrabAttack();
                if (playerHits >= maxPlayerHits)
                {
                    KnockbackPlayer();
                    state = "patrol";
                    stateTimer = 0f;
                    playerHits = 0;
                    grabHits = 0;
                }
                break;

            case "stun":
                Stun();
                if (stateTimer >= 2f)
                {
                    state = "patrol";
                    stateTimer = 0f;
                }
                break;
        }
    }

    private void Patrol()
    {
        animator.SetBool("isWalking", true);
        int layer = LayerMask.GetMask("Ground", "Destructable");

        // Check for obstacles
        Vector2 rayDirection = direction > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 2, layer);

        if (hit.collider != null)
        {
            direction *= -1;
            Flip(direction);
        }

        rb.velocity = new Vector2(patrolSpeed * direction, rb.velocity.y);
    }

    private void StartDash()
    {
        isDashing = true;
        dashHitPlayer = false;
        lastDashTime = Time.time;
        stateTimer = 0f;

        animator.SetBool("isDashing", true);
        Vector2 dashDirection = (player.position - transform.position).normalized;
        rb.velocity = dashDirection * dashSpeed;

        dustParticles.Play();
    }

    private void EndDash()
    {
        isDashing = false;
        animator.SetBool("isDashing", false);
        rb.velocity = Vector2.zero;
    }

    private void GrabAttack()
    {
        animator.SetBool("isGrabbing", true);
        rb.velocity = Vector2.zero;

        if (distanceToPlayer <= 1.5f)
        {
            // Move the player to the level of the head of the mantis
            Vector3 playerPosition = player.position;
            playerPosition.y = transform.position.y + 1.5f; // Adjust the y value as needed
            player.position = playerPosition;

            // Deal damage to the player if cooldown has passed
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                player.GetComponent<Player>().TakeDamage(attackDamage);
                lastAttackTime = Time.time;
                grabHits++;
            }
        }
    }

    private void KnockbackPlayer()
    {
        Vector2 knockbackDirection = (player.position - transform.position).normalized;
        player.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void Stun()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("isStunned", true);
    }

    private void Flip(float movementDirection)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(movementDirection) * Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isDashing)
        {
            other.GetComponent<Player>().TakeDamage(attackDamage);
            dashHitPlayer = true;
        }
    }

    public void PlayerHitMantis()
    {
        playerHits++;
        if (playerHits >= maxPlayerHits)
        {
            KnockbackPlayer();
            state = "patrol";
            stateTimer = 0f;
            playerHits = 0;
            grabHits = 0;
        }
    }
}