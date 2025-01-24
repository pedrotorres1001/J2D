using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject bossHealthBar;
    public int health = 800;
    public int maxHealth = 800;
    public int damage = 10;

    public int experiencePoints;

    public int attackDamage;

    // Movement properties
    public float speed;
    public Rigidbody2D rb;
    public bool engaged = false;

    public GameObject leftBoundary;
    public GameObject rightBoundary;

    public AudioManager audioManager;
    public AudioSource SFXSource;
    public AudioSource LoopSource;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        StartCoroutine(ColorChangeCoroutine());

        bossHealthBar.GetComponent<HealthBar>().Update_health(health, maxHealth);

        audioManager.Play(SFXSource, "enemyDeath");

        if (health <= 0)
        {
            Die();
        }
    }

    public IEnumerator ColorChangeCoroutine()
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

    void Die()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddExperiencePoints(experiencePoints);
        bossHealthBar.SetActive(false);
        Destroy(gameObject);

    }
}
