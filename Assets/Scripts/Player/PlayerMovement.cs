using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float stairsForce;
    [SerializeField] private bool isGrounded;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float gravity;
    [SerializeField] private GameObject player;

    private Animator animator;
    private Rigidbody2D rb;
    public float lastDirection = 1;

    private bool isOnStairs;
    private bool upPressed;

    public float direction;
    public float altitude;

    private bool canWallJump = false;
    private bool isGrappling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isOnStairs = false;
        lastDirection = -1;
        animator.SetFloat("LastDirection", lastDirection);
    }

    void Update()
    {
        isGrappling = player.GetComponent<GrapplingHook>();
        direction = Input.GetAxisRaw("Horizontal");
        altitude = Input.GetAxisRaw("Vertical");

        CheckGround();

        if (direction != 0)
        {
            animator.SetFloat("Direction", direction);
            animator.SetFloat("LastDirection", lastDirection);
            lastDirection = direction;
            animator.SetBool("IsWalking", true);
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            lastDirection = 2;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            lastDirection = 3;
        }

        if (direction == 0)
        {
            animator.SetFloat("Direction", direction);
            animator.SetFloat("LastDirection", lastDirection);
            animator.SetBool("IsWalking", false);
        }

        if ((Input.GetButton("Jump") || Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            Jump();
        }

        // Handle wall jump
        if (Input.GetButtonDown("Jump") && canWallJump && !isGrounded && !isOnStairs)
        {
            WallJump();
        }

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isOnStairs)
        {
            upPressed = true;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        if (upPressed)
        {
            rb.velocity = new Vector3(rb.velocity.x, altitude * speed);
            rb.gravityScale = 0; 
            upPressed = false;
        }
        else
        {
            rb.gravityScale = gravity;
        }
    }

    void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        isGrounded = hit.collider != null;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false; // Ensures that jump only happens once per ground contact
    }

    private void WallJump()
    {
        // Apply a force to simulate a wall jump
        Vector2 wallJumpDirection = new Vector2(-Mathf.Sign(transform.localScale.x) * 7f, 9f); // Apply force in the opposite direction of the wall
        rb.velocity = new Vector2(wallJumpDirection.x, wallJumpDirection.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Stairs")) 
        {
            isOnStairs = true;
        }

        if (other.CompareTag("Ground") && !isGrounded && !isOnStairs)
        {
            canWallJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Stairs")) 
        {
            isOnStairs = false;
        }

        if (other.CompareTag("Ground"))
        {
            canWallJump = false;
        }
    }
}