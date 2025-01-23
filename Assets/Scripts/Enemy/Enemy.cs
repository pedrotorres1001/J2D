using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health = 100;
    public int Health {get;set;}
    [SerializeField] protected int maxHealth = 100;
    public GameObject health_bar;
    [SerializeField] protected float speed;
    [SerializeField] protected float sightRange;
    [SerializeField] GameObject crystal;
    [SerializeField] protected int experiencePoints;
    [SerializeField] private GameObject deathPrefab;


    private AudioManager audioManager;

    protected virtual void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        //health_bar.transform.position = new Vector3(0, .5f);
        //health_bar = Instantiate(health_bar, transform);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        //audioManager.PlaySFX(audioManager.enemyDeath);
        StartCoroutine(ColorChangeCoroutine());

        //health_bar.GetComponent<HealthBar>().Update_health(health, maxHealth);

        if (health <= 0)
        {
            Die();
            Destroy(health_bar);
        }
    }

    void Die()
    {
        // Play death sound
        audioManager.Play("enemyDeath");

        Instantiate(deathPrefab, transform.position, Quaternion.identity);
        for (int i = 0; i < experiencePoints; i++)
        {
            float randomX = Random.Range(transform.position.x-1, transform.position.x + 1);
            Instantiate(crystal, new Vector3(randomX, transform.position.y, transform.position.z), Quaternion.identity);
        }

            Destroy(gameObject);
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