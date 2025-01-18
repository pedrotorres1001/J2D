using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health = 100;
    [SerializeField] protected int maxHealth = 100;
    
    public GameObject health_bar;

    [SerializeField] protected int experiencePoints;
    [SerializeField] protected float speed;

    [SerializeField] private GameObject deathPrefab;

    private AudioManager audioManager;
    public bool isAlive;

    public int Health { 
        get { return health;  }
        set { health = value; }}

    protected virtual void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        health_bar.transform.position = new Vector3(0, .7f);
        health_bar = Instantiate(health_bar, transform);

        isAlive = true;
    }

    public abstract void Attack();

    public void TakeDamage(int damage)
    {
        health -= damage;

        StartCoroutine(ColorChangeCoroutine());

        health_bar.GetComponent<HealthBar>().Update_health(health, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Play death sound
        audioManager.Play("enemyDeath");

        // Add experience points to the player
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddExperiencePoints(experiencePoints);

        // Instantiate the death prefab at the current position and rotation

        if (deathPrefab != null)
        {
            Instantiate(deathPrefab, transform.position, Quaternion.identity);
        }

        // Destroy the enemy
        isAlive = false;
        gameObject.SetActive(false);
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


}