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

    private void Start() 
    {
        health = maxHealth;
        experience = 0;
    }

    public int getMaxExperience()
    {
        return maxExperience;
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
        yield return new WaitForSeconds(0.1f);
        sprite.color = original;
        yield return new WaitForSeconds(0.1f);
        sprite.color = damaged;
        yield return new WaitForSeconds(0.1f);
        sprite.color = original;
        yield return new WaitForSeconds(0.1f);
        sprite.color = damaged;
        yield return new WaitForSeconds(0.1f);
        sprite.color = original;
    }
}