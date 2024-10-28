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

    private Animator animator;
    private Rigidbody2D rb;
    private float lastDirection = 1;

    private bool isOnStairs;

    public float direction;
    public float altitude;

    // Start is called before the first frame update


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isOnStairs = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from the user
        direction = Input.GetAxis("Horizontal");
        altitude = Input.GetAxis("Vertical");

        // Apply the movement to the character
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        // checkar se está no chão com raycast
        CheckGround();

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded && !isOnStairs)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
        else if(Input.GetKey(KeyCode.W) && isOnStairs)
        {
            rb.AddForce(new Vector3(0, stairsForce, 0), ForceMode2D.Impulse);
        }

        if (direction != 0)
        {
            animator.SetFloat("MoveX", direction);
            lastDirection = direction;
        }

        if (direction == 0)
        {
            animator.SetFloat("MoveX", lastDirection);
        }

    }

    void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

/*     void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position +  Vector3.down * groundCheckDistance);
    } */

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Stairs")) 
        {
            isOnStairs = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Stairs")) 
        {
            isOnStairs = false;
        }
    }
}