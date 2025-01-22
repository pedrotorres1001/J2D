using UnityEngine;
using System.Collections;
using System;

public class BossMovement : Boss
{
    [SerializeField] private GameObject attackTrigger;

    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float prepareCooldown = 2f;
    [SerializeField] private float stunCooldown = 1f;
    [SerializeField] public float dashCooldown = 1f;
    private float cooldown;
    private float lastAttackTime;

    // Movement properties
    private bool facingRight = false;

    private Transform player;

    // Dash properties
    [SerializeField] private float dashSpeed = 10f;
    private bool isDashing = false;

    private float moveDirection;
    private Vector2 dashDirection;
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
        float yDifference = Mathf.Abs(transform.position.y - player.position.y);
        float directionX;


        switch (engaged) 
        {
            case true:
                switch (state)
                {
                    case "idle":
                        if (yDifference <= 3f)
                        {
                            animator.SetBool("isWalking", true);

                            directionX = player.position.x - transform.position.x;
                            FlipTowardsPlayer(directionX);

                            if (distanceToPlayer <= 5)
                            {
                                Vector2 direction = (transform.position - player.position).normalized;
                                if ((direction.x < 0 && transform.position.x <= rightBoundary.transform.position.x && Vector2.Distance(transform.position, leftBoundary.transform.position) >= 5) 
                                || (direction.x > 0 && transform.position.x >= leftBoundary.transform.position.x && Vector2.Distance(transform.position, rightBoundary.transform.position) >= 5))
                                {
                                    rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
                                }
                                else
                                {
                                    cooldown = prepareCooldown;
                                    gameObject.GetComponent<Animator>().SetTrigger("attack");
                                    state = "predash";
                                    animator.SetBool("isWalking", false);
                                    animator.SetBool("startCharge", true);
                                }
                            }
                            else if (distanceToPlayer <= 7)
                            {
                                cooldown = prepareCooldown;
                                gameObject.GetComponent<Animator>().SetTrigger("attack");
                                state = "predash";
                                animator.SetBool("isWalking", false);
                                animator.SetBool("startCharge", true);
                            }
                            else if (distanceToPlayer > 7)
                            {
                                FollowPlayer();
                            }
                        }
                        else
                        {
                            animator.SetBool("isWalking", false);
                            MovementWithinBoundaries();
                        }
                        break;
                    case "predash":
                        rb.velocity = Vector2.zero;
                        directionX = player.position.x - transform.position.x;
                        FlipTowardsPlayer(directionX);
                        if (cooldown == 0)
                        {
                            isDashing = false;
                            attackTrigger.SetActive(true);
                            state = "dash";
                            animator.SetBool("startCharge", false);
                            animator.SetBool("isCharging", true);
                        }

                        break;
                    case "dash":
                        if (!isDashing)
                        {
                            if (player.position.x - transform.position.x <= 0)
                            {
                                dashDirection.x = -1;
                            }
                            else
                            {
                                dashDirection.x = 1;

                            }
                            dashDirection.y = 0;
                            isDashing = true;
                        }

                        rb.velocity = dashDirection * dashSpeed;
                        break;
                    case "postdash":
                        animator.SetBool("isCharging", false);
                        animator.SetBool("isStunned", true);
                        animator.Play("MiniBossStun1");
                        if (cooldown == 0)
                        {
                            state = "idle";
                            animator.SetBool("isStunned", false);
                        }
                        break;
                    case "hit":
                        if (cooldown == 0)
                            state = "idle";

                        if (transform.transform.position.x > leftBoundary.transform.position.x 
                        && transform.transform.position.x < rightBoundary.transform.position.x)
                        {
                            // Gradually decrease speed
                            rb.velocity = rb.velocity * .9f;
                        }
                        else
                        {
                            rb.velocity = Vector2.zero;
                        }

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
            Vector2 projectionForce = (player.position - transform.position).normalized * 10000f;
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

    internal void ChangeState(string state)
    {
        if (state == "postdash")
        {
            cooldown = dashCooldown * 1.5f;
            rb.velocity = Vector2.zero;
            animator.SetBool("isCharging", false);
        }
        else if (state == "hit")
        {
            cooldown = dashCooldown;
            animator.SetBool("isCharging", false);
        }
        this.state = state;
    }
}