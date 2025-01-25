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

    private string state = "patrol";
    private float stateTimer = 0f;

    protected override void Start() {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        eyeCollider = gameObject.GetComponentInChildren<EyeCollider>();

        direction = 1;
        colidedWithPlayer = false;
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
                if (canSeePlayer && distanceToPlayer > 6) {
                    state = "dash";
                    stateTimer = 0f;
                }
                break;

            case "dash":
                Dash();
                if (stateTimer >= dashDuration) {
                    state = "grab";
                    stateTimer = 0f;
                }
                break;

            case "grab":
                GrabAttack();
                if (stateTimer >= 1f) {
                    state = "patrol";
                    stateTimer = 0f;
                }
                break;

            case "stun":
                Stun();
                if (stateTimer >= 2f) {
                    state = "patrol";
                    stateTimer = 0f;
                }
                break;

            default:
                break;
        }

        if (gameObject.GetComponent<Rigidbody2D>().velocity.x != 0 && timer >= interval)
        {
            dustParticles.Play(); // Reproduz as partículas
            timer = 0f; // Reseta o temporizador   
        }
    }

    private void Patrol()
    {
        animator.SetBool("isWalking", true);
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

    private void Dash()
    {
        animator.SetBool("isDashing", true);
        Vector2 dashDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;
        rb.velocity = dashDirection * dashSpeed;
    }

    private void GrabAttack()
    {
        animator.SetBool("isGrabbing", true);
        rb.velocity = Vector2.zero;
    }

    private void Stun()
    {
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            colidedWithPlayer = true;
            state = "stun";
            stateTimer = 0f;
        }
    }

    private void Flip(float movementDirection)
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}