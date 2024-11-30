using UnityEngine;
using System.Collections;
using System;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private GameObject bossHealthBar;
    public int health = 800;
    [SerializeField] public int maxHealth = 800;
    [SerializeField] public int damage = 10;

    [SerializeField] private int experiencePoints;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject attackTrigger;

    [SerializeField] public int attackDamage;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float prepareCooldown = 2f;
    [SerializeField] private float stunCooldown = 1f;
    [SerializeField] public float dashCooldown = 1f;
    private float cooldown;
    private float lastAttackTime;

    // Movement properties
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    public bool engaged = false;

    private Transform player;

    // Dash properties
    [SerializeField] private float dashSpeed = 10f;
    private bool isDashing = false;

    private float moveDirection;
    private Vector2 dashDirection;

    [SerializeField] private GameObject leftBoundary;
    [SerializeField] private GameObject rightBoundary;

    [SerializeField] private string state;

    AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

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

        switch (engaged) 
        {
        case true:
            switch (state)
            {
            case "idle":
                if (yDifference <= 3f)
                {
                    float directionX = player.position.x - transform.position.x;
                    FlipTowardsPlayer(directionX);

                    if (distanceToPlayer <= 4.5f)
                    {
                        Vector2 direction = (transform.position - player.position).normalized;
                        if ((direction.x < 0 && transform.position.x <= rightBoundary.transform.position.x) 
                        || (direction.x > 0 && transform.position.x >= leftBoundary.transform.position.x))
                        {
                            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
                        }
                        else
                        {
                            cooldown = prepareCooldown;
                            gameObject.GetComponent<Animator>().SetTrigger("attack");
                            state = "predash";
                        }
                    }
                    else if (distanceToPlayer <= 7)
                    {
                        cooldown = prepareCooldown;
                        gameObject.GetComponent<Animator>().SetTrigger("attack");
                        state = "predash";
                    }
                    else if (distanceToPlayer > 7)
                    {
                        FollowPlayer();
                    }
                }
                else
                {
                    MovementBoundaries();
                }
                break;
            case "predash":
                rb.velocity = Vector2.zero;
                if (cooldown == 0)
                {
                    isDashing = false;
                    attackTrigger.SetActive(true);
                    state = "dash";
                }

                break;
            case "dash":
                if (!isDashing)
                {
                    dashDirection = (player.position - transform.position).normalized;
                    dashDirection.y = 0;
                    rb.velocity = dashDirection * dashSpeed;
                    isDashing = true;
                }

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
                }

                break;
            case "postdash":
                if (cooldown == 0)
                    state = "idle";
                break;
            case "hit":
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

    public void MovementBoundaries()
    {        
        if (transform.position.x >= rightBoundary.transform.position.x)
        {
            moveDirection = -1;
            if (facingRight) Flip();
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else if (transform.position.x <= leftBoundary.transform.position.x)
        {
            moveDirection = 1;
            if (!facingRight) Flip();
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    public IEnumerator ProjectPlayer()
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            Vector2 projectionForce = (player.position - transform.position).normalized * 20f;
            playerRb.AddForce(projectionForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.1f); // Pequena espera para estabilizar

            // Aplica uma força contínua enquanto o jogador está grappling
            while (true)
            {
                playerRb.AddForce(projectionForce, ForceMode2D.Impulse); // Força contínua
                yield return null; // Espera até o próximo frame
            }
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

        bossHealthBar.GetComponent<HealthBar>().Update_health(health, maxHealth);

        audioManager.PlaySFX(audioManager.enemyDeath);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddExperiencePoints(experiencePoints);
        bossHealthBar.SetActive(true);
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

    internal void ChangeState(string state)
    {
        if (state == "postdash")
        {
            cooldown = dashCooldown;
            rb.velocity = Vector2.zero;
        }
        this.state = state;
    }
}