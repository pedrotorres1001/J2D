using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class Boss2 : Boss
{
    [SerializeField] private float attackCooldown = 2f;
    private float cooldown;
    private float lastAttackTime;
    public bool inAttackRange = false;

    // Movement properties
    private float moveDirection;
    private bool facingRight = false;

    private Transform player;

    private Animator animator;

    [SerializeField] private string state;

    AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        animator = gameObject.GetComponent<Animator>();

        engaged = false;
        health = maxHealth;
        cooldown = 0;
        state = "idle";
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
        float yDifference = Mathf.Abs(transform.position.y - 1.6f - player.position.y);

        if (yDifference <= 3f)
        {
            state = "melee";
        }
        else
        {
            state = "ranged";
        }

        switch (engaged)
        {
            case true:
                switch (state)
                {
                    case "melee":
                        if (inAttackRange)
                        {
                            Debug.Log("true");
                            Attack();
                        }
                        else
                        {
                            animator.SetBool("isMelee", false);
                            animator.SetBool("isWalking", true);
                            FollowPlayer();
                        }
                        break;

                    case "ranged":
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }


        cooldown = Mathf.Max(0, cooldown - Time.deltaTime);
        if (cooldown == 2)
            Debug.Log($"Cooldown: {cooldown}");


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
            animator.SetBool("isWalking", false);
            animator.SetBool("isMelee", true);

            player.GetComponent<Player>().TakeDamage(attackDamage);
            ProjectPlayer();
            lastAttackTime = Time.time;
        }
    }

    public void MovementWithinBoundaries()
    {
        if (cooldown == 0 && facingRight && Vector2.Distance(transform.position, rightBoundary.transform.position) >= 4)
        {
            animator.SetBool("isWalking", true);
            moveDirection = 1;
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (Vector2.Distance(transform.position, rightBoundary.transform.position) <= 5)
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("isWalking", false);
                cooldown = 1.2f;
            }
        }
        else if (cooldown == 0 && !facingRight && Vector2.Distance(transform.position, leftBoundary.transform.position) >= 4)
        {
            animator.SetBool("isWalking", true);
            moveDirection = -1;
            rb.velocity = new Vector2(-speed, rb.velocity.y);

            if (Vector2.Distance(transform.position, leftBoundary.transform.position) <= 5)
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("isWalking", false);
                cooldown = 1.2f;
            }
        }
        else if (cooldown == 0)
        {
            Flip();
        }

        cooldown = Mathf.Max(0, cooldown - Time.deltaTime);

    }

    public void ProjectPlayer()
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            Vector2 projectionForce = (player.position - transform.position).normalized * 100f;
            projectionForce.y = 1f;
            playerRb.AddForce(projectionForce, ForceMode2D.Force);
        }
        else
        {
            Debug.LogWarning("Player Rigidbody2D not found!");
        }
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

    public void TakeDamage(int damage)
    {
        health -= damage;

        StartCoroutine(ColorChangeCoroutine());

        bossHealthBar.GetComponent<HealthBar>().Update_health(health, maxHealth);

        audioManager.Play("enemyDeath");

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator ColorChangeCoroutine()
    {
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        Color damaged = Color.red;
        Color original = Color.white;

        // Change the color
        sprite.color = damaged;

        // Wait for the duration
        yield return new WaitForSeconds(0.5f);

        // Revert to the original color
        sprite.color = original;
    }

    void Die()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddExperiencePoints(experiencePoints);
        bossHealthBar.SetActive(false);
        Destroy(gameObject);

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