using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float leftBoundary;
    [SerializeField] private float rightBoundary;

    private Rigidbody2D rb1;
    private float moveDirection = 1;

    // Start is called before the first frame update
    void Start()
    {
        GameObject enemy1 = GameObject.FindWithTag("Enemy1");

        if (enemy1 != null)
        {
            rb1 = enemy1.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move the enemy within the boundaries
        if (transform.position.x >= rightBoundary)
        {
            moveDirection = -1;
        }
        else if (transform.position.x <= leftBoundary)
        {
            moveDirection = 1;
        }

        rb1.velocity = new Vector2(moveDirection * speed, rb1.velocity.y);
    }
}