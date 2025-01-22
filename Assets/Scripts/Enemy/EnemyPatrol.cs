using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float patrolSpeed;
    private Animator animator;
    private float direction; // Positive = right, Negative = left
    public bool isFollowingPlayer;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        // Start moving to the right initially
        direction = 1f;
        isFollowingPlayer = false;
    }

    public void Patrol(Rigidbody2D rb)
    {
        int layer = LayerMask.GetMask("Ground", "Destructable");

        // Check for obstacles in the current direction
        Vector2 rayDirection = direction > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 2, layer);

        if (hit.collider != null)
        {
            // Reverse direction if an obstacle is detected
            direction *= -1;
        }

        // Apply movement based on the current direction
        rb.velocity = new Vector2(patrolSpeed * direction, rb.velocity.y);

        // Update the animator with the patrol direction
        animator.SetFloat("Direction", direction);
    }
}