using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{

    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] public int attackDamage;

    private bool hasDashed = false;
    private float lastAttackTime;
    private Transform player;
    private float distanceToPlayer;
    private Rigidbody2D rb;
    private bool isDashing;
    private EnemyPatrol enemyPatrol;
    private Animator animator;

    private bool isPerformingAction;
    private bool isChasingPlayer;
    private Vector2 playerLastKnownPosition;

    private string currentState;
    private int facingDirection = 1;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        enemyPatrol = gameObject.GetComponent<EnemyPatrol>();
        animator = gameObject.GetComponent<Animator>();
    }

    protected override void Start() {
        base.Start();
        isDashing = false;
        isPerformingAction = false;
        isChasingPlayer = false;
    }

private void Update() 
{
    print(currentState);
    
    if (isPerformingAction) return; // Skip logic during special actions

    playerLastKnownPosition = player.position;

    distanceToPlayer = Vector2.Distance(transform.position, player.position);

    bool canSeePlayer = HasLineOfSight();
    
    if (canSeePlayer && !isChasingPlayer && !isDashing) 
    {
        StartCoroutine(PrepareAndDash());
    }
    else if (!canSeePlayer) 
    {
        currentState = "Patrolling";
        isChasingPlayer = false;
        enemyPatrol.Patrol(rb);
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

        currentState = "Following Player";
    }

    private bool HasLineOfSight()
    {
        float boxcastRange = 4f;
        float boxWidth = 7f;
        float boxHeight = 5f;
        float direction = rb.velocity.x > 0 ? 1 : -1;

        // Define as camadas a considerar (Player + Ground ou outros obstáculos)
        LayerMask layerMask = LayerMask.GetMask("Player", "Ground", "Destructable");

        // Faz um BoxCast na direção que o Boar está a olhar
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position,
            new Vector2(boxWidth, boxHeight),
            0f,
            Vector2.right * direction,
            boxcastRange,
            layerMask
        );

        // Verifica o que foi atingido
        if (hit.collider != null)
        {
            // Se atingir o jogador diretamente, retorna true
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            // Se atingir um obstáculo (ex: Ground), retorna false
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                return false;
            }
        }

        return false;
    }

    private bool CanReachPlayer()
    {
        float direction = rb.velocity.x > 0 ? 1 : -1; // Determine facing direction
        float range = 2f;

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y - 1f), transform.position * (direction + range), LayerMask.GetMask("Player"));

        // Se o Raycast acertar o jogador, o jogador está visível
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }

    public override void Attack()
    {
        print("5. Attacking player");
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, 1f);
            
            foreach (Collider2D target in hitPlayers)
            {
                if (target.CompareTag("Player"))
                {
                    print("5. Attacking player");
                    rb.velocity = Vector2.zero;
                    target.GetComponent<Player>().TakeDamage(attackDamage);
                }
            }
            lastAttackTime = Time.time;
        }
    }

    private IEnumerator PrepareAndDash() 
    {
        currentState = "Charging player";
        isPerformingAction = true;

        // Record the player's position before charging

        // Calculate dash direction (X-axis only)
        Vector2 dashDirection = new Vector2(playerLastKnownPosition.x - transform.position.x, 0).normalized;

        float direction = dashDirection.x > 0 ? 1 : -1; // Determine facing direction
        animator.SetFloat("Direction", direction); // Update animation direction

        // Stop and wait before charging
        animator.SetBool("isPreparing", true);

        print("3. Preparing to charge");

        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1); // Wait before charging
        animator.SetBool("isPreparing", false);
        animator.SetBool("isCharging", true);

        print("4. Charging");

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

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Attack();
        }
    }

    private void OnDrawGizmos()
    {
        if (transform != null)
        {
            // Direção do Boar
            rb = gameObject.GetComponent<Rigidbody2D>();
            float direction = rb.velocity.x > 0 ? 1 : -1; // Determine facing direction
            Vector2 directionV = new Vector2(direction, 0);

            // Gizmos para HasLineOfSight
            Gizmos.color = Color.green; // Cor para a área de visão
            Vector2 boxSize = new Vector2(7f, 5f); // Tamanho da caixa (HasLineOfSight)
            Gizmos.DrawWireCube(transform.position + (Vector3)(directionV * 4f), boxSize);

        }
    }

}