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
    [SerializeField] int maxExperience;
    [SerializeField] Animator animator;
    [SerializeField] GameObject gameSceneManager;
    private bool isAlive;
    private bool isBlinking;

    private void Start() 
    {
        health = maxHealth;
        experience = 0;
        isAlive = true;
        isBlinking = false;
    }

    public int getMaxExperience()
    {
        return maxExperience;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(!isBlinking)
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
        isAlive = false;
        animator.SetBool("IsDead", true);
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(WaitDeath());
    }

    private IEnumerator ColorChangeCoroutine()
    {
        isBlinking = true;

        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        Color damaged = Color.red;
        Color original = gameObject.GetComponent<SpriteRenderer>().color;

        for(int i = 0; i < 5; i++) {
            yield return new WaitForSeconds(0.1f);
            sprite.color = damaged;
            yield return new WaitForSeconds(0.1f);
            sprite.color = original;

        }
        isBlinking = false;
    }

    private IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(2f);
        isAlive = true;
        gameObject.GetComponent<PlayerMovement>().enabled = true;
        animator.SetBool("IsDead", false);
        health = maxHealth;
        transform.position = new Vector2(PlayerPrefs.GetFloat("FirstRespawnX"), PlayerPrefs.GetFloat("FirstRespawnY"));

    }
}