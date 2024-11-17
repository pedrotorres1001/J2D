using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int pickaxeLevel;
    public int experience;
    [SerializeField] private int maxExperience;

    private void Start() {
        health = maxHealth;
        experience = 0;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        StartCoroutine(ColorChangeCoroutine());

        if (health <= 0)
        {
            Die();
        }
    }

    public void AddExperiencePoints(int points)
    {
        experience += points;

        if(experience >= maxExperience) 
        {
            pickaxeLevel++;
        }
    }

    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator ColorChangeCoroutine()
    {
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        Color damaged = Color.red;
        Color original = Color.white;

        // Change the color
        sprite.color = damaged;

        // Wait for the duration
        yield return new WaitForSeconds(0.5f);

        // Revert to the original color
        sprite.color = original;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            ApplyKnockback(collision);
        }
    }

    bool isInvulnerable = false;
    
    void ApplyKnockback(Collision2D collision)
    {
        if (!isInvulnerable)
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position);
            knockbackDirection.y = 0; // Eliminate vertical component
            knockbackDirection = knockbackDirection.normalized; // Normalize for consistent direction

            float knockbackForce = 1f;
            gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
}