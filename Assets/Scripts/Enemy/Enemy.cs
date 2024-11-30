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
    [SerializeField] protected float sightRange;

    private AudioManager audioManager;

    protected virtual void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        //health_bar.transform.position = new Vector3(0, .5f);
        //health_bar = Instantiate(health_bar, transform);
    }

    public abstract void Attack();

    public void TakeDamage(int damage)
    {
        health -= damage;

        audioManager.PlaySFX(audioManager.enemyDeath);
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
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddExperiencePoints(experiencePoints);
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