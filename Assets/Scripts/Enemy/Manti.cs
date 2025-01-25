using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manti : Enemy
{
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1.5f;
    [SerializeField] public int attackDamage;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] ParticleSystem dustParticles;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource LoopSource;

    private float lastDashTime;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    private EyeCollider eyeCollider;
    private bool canSeePlayer;
    private float distanceToPlayer;

    private bool isDashing;
    private bool dashHitPlayer;

    private string state = "patrol";
    private float stateTimer;

    private int direction = 1;
    private float patrolSpeed = 3f;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        eyeCollider = GetComponentInChildren<EyeCollider>();

        lastDashTime = -dashCooldown; // Allow immediate dash at start
        currentHealth = maxHealth; // Initialize health
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
                if (stateTimer >= 1f)
                {
                    state = "patrol";
                    stateTimer = 0f;
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
            player.GetComponent<Player>().TakeDamage(attackDamage);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isDashing)
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(attackDamage);
            dashHitPlayer = true;
        }

        // Handle damage from player attacks
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            int damage = collision.gameObject.GetComponent<Player>().health;
            TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("isHurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("isDead");
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        // Play death particles or SFX
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f); // Show grab range
    }
}
