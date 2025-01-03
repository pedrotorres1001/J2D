using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    [SerializeField] private int vitalDamageMultiplier; 
    [SerializeField] private float pullStrength = 10f; // Customize pull speed

    private Transform player;
    private Rigidbody2D playerRb;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Apply regular damage
            other.GetComponent<Enemy>().TakeDamage(attackDamage);

            // Pull player toward the enemy
            PullPlayer(other.transform);
        }
        else if (other.CompareTag("Vital"))
        {
            // Apply vital damage
            Transform enemyTransform = other.transform.parent;
            if (enemyTransform != null && enemyTransform.CompareTag("Enemy"))
            {
                enemyTransform.GetComponent<Enemy>().TakeDamage(attackDamage * vitalDamageMultiplier);

                // Pull player toward the vital part
                PullPlayer(enemyTransform);
            }
        }
    }

    // Pull the player toward the enemy's position
    private void PullPlayer(Transform target)
    {
        // Calculate direction toward the enemy
        Vector2 pullDirection = (target.position - player.position).normalized;

        // Apply force if the player has Rigidbody2D
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero; // Reset current velocity
            playerRb.AddForce(pullDirection * pullStrength, ForceMode2D.Impulse);
        }
        else
        {
            // Smoothly move the player toward the enemy if no Rigidbody2D
            StartCoroutine(SmoothPull(target));
        }
    }

    // Smoothly move player to the enemy if Rigidbody2D isn't used
    private IEnumerator SmoothPull(Transform target)
    {
        float pullDuration = 0.5f; // Adjust duration for smoother pull
        float elapsedTime = 0f;

        Vector2 startPosition = player.position;
        Vector2 endPosition = target.position;

        while (elapsedTime < pullDuration)
        {
            elapsedTime += Time.deltaTime;
            player.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / pullDuration);
            yield return null;
        }
    }

}
