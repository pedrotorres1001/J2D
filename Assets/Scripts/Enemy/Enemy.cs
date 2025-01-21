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

    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject crystal;


    private AudioManager audioManager;
    public bool isAlive;

    public GameObject healthBarFill;
    private Vector3 originalPosition; 
    private float originalWidth;

    public int Health { 
        get { return health;  }
        set { health = value; }}

    protected virtual void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        isAlive = true;

        originalPosition = healthBarFill.transform.position;
        originalWidth = healthBarFill.GetComponent<SpriteRenderer>().bounds.size.x;

    }

    public abstract void Attack();

    public void TakeDamage(int damage)
    {
        health -= damage;

        StartCoroutine(ColorChangeCoroutine());

        health_bar.GetComponent<HealthBar>().Update_health(health, maxHealth);

        UpdateHealthBar();

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        audioManager.Play("enemyDeath");

        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Instantiate(crystal, transform.position, Quaternion.identity);

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

    private void UpdateHealthBar()
    {

        float healthPercentage = (float)health / maxHealth;

        // Atualiza a escala para a porcentagem de saúde
        healthBarFill.transform.localScale = new Vector3(healthPercentage, 1, 1);

        // Ajusta a posição para manter a barra ancorada à esquerda
        Vector3 newPosition = healthBarFill.transform.position;
        newPosition.x = originalPosition.x - (healthBarFill.transform.localScale.x * originalWidth) / 2;
        healthBarFill.transform.position = newPosition;
    }


}