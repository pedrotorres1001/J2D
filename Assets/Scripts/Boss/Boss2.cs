using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class Boss2 : Boss
{
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float projectileSpeed = 4.5f;
    private float cooldown;
    private float lastAttackTime;
    public bool inAttackRange = false;

    // Movement properties
    private float moveDirection;
    private bool facingRight = false;

    public bool isAttacking = false;

    private Transform player;

    private Animator animator;

    [SerializeField] private string state;
    [SerializeField] private GameObject projectileSpawnPoint;
    [SerializeField] private GameObject projectile;


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
        Vector3 direction = (player.position - transform.position);


        if (hasShield && !isAttacking)
        {
            animator.SetBool("isMelee", false);
            animator.SetBool("isRanged", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isShield", true);
            state = "shield";
        }
        else if (yDifference <= 1.5f && distanceToPlayer <= 4)
        {

            state = "melee";
        }
        else if (!isAttacking)
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
                            animator.SetBool("isRanged", false);
                            Attack();
                        }
                        else if (!isAttacking)
                        {
                            animator.SetBool("isMelee", false);
                            animator.SetBool("isWalking", true);
                            FollowPlayer();
                        }
                        break;

                    case "ranged":
                        if (Vector2.Distance(transform.position, player.position) <= 6
                        && ((direction.x < 0 && transform.position.x <= rightBoundary.transform.position.x && Vector2.Distance(transform.position, leftBoundary.transform.position) >= 5)
                        || (direction.x > 0 && transform.position.x >= leftBoundary.transform.position.x && Vector2.Distance(transform.position, rightBoundary.transform.position) >= 5)))
                        {
                            direction.y = 0;
                            direction = direction.normalized;
                            rb.velocity = new Vector2(-direction.x * speed, rb.velocity.y);

                            FlipTowardsPlayer(direction.x);
                        }
                        else
                        {
                            if (Time.time - lastAttackTime >= attackCooldown)
                            {
                                lastAttackTime = Time.time;

                                Vector3 dir = (player.transform.position - projectileSpawnPoint.transform.position).normalized;

                                animator.SetBool("isRanged", true);
                                animator.SetBool("isWalking", false);
                                animator.Play("Boss2AttackRanged", -1, 0f);

                                GameObject proj = Instantiate(projectile);
                                proj.transform.position = projectileSpawnPoint.transform.position;
                                proj.GetComponent<Projectile>().SetValues(dir, damage, projectileSpeed);
                                audioManager.Play(SFXSource, "spray");
                                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                                proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                                proj.transform.Rotate(0, 0, 135);
                            }
                        }
                        break;
                    case "shield":

                        if (Time.time - lastDamageTime >= shieldCooldown)
                        {
                            animator.SetBool("isShield", false);
                            hasShield = false;
                        }
                        Vector3 playerDirection = (player.position - transform.position);
                        playerDirection.y = 0;
                        playerDirection = playerDirection.normalized;
                        FlipTowardsPlayer(playerDirection.x);

                        transform.position += -playerDirection * speed * 1.5f * Time.deltaTime;
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
        if (Time.time - lastAttackTime >= attackCooldown && !isAttacking)
        {
            isAttacking = true; 
            animator.SetBool("isWalking", false);
            animator.SetBool("isMelee", true);
            animator.Play("Boss2AttackMelee", -1, 0f);
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