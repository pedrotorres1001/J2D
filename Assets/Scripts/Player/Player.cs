using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;

        StartCoroutine(ColorChangeCoroutine());

        if (health <= 0)
        {
            Die();
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
}