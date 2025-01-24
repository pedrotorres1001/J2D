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
    public bool hasShield = false;
    public float shieldCooldown = 4.5f;
    public float lastDamageTime;

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
        if (Time.time - lastDamageTime >= shieldCooldown)
        {
            int currentHP = health;
            

            health -= damage;

            if ((currentHP > maxHealth * 0.66f && health <= maxHealth * 0.66f)
                || (currentHP > maxHealth * 0.33f && health <= maxHealth * 0.33f))
            {
                hasShield = true;
                lastDamageTime = Time.time;
            }

            StartCoroutine(ColorChangeCoroutine());

            bossHealthBar.GetComponent<HealthBar>().Update_health(health, maxHealth);

            audioManager.Play(SFXSource, "enemyDeath");

            if (health <= 0)
            {
                Die();
            }
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
        audioManager.PlayMusic("track1");

        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddExperiencePoints(experiencePoints);
        bossHealthBar.SetActive(false);
        Destroy(gameObject);

    }
}
