using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : MonoBehaviour
{
    public GameObject bossHealthBar;
    public int health = 800;
    public int maxHealth = 800;
    public int damage = 10;

    public int experiencePoints;

    // Movement properties
    public bool engaged = false;
    public bool hasShield = false;
    public float shieldCooldown = 4.5f;
    public float lastDamageTime;

    [SerializeField] GameObject vital;
    [SerializeField] float projectionForce = 9800f;


    public AudioManager audioManager;
    public AudioSource SFXSource;
    public AudioSource LoopSource;

    [SerializeField] GameObject[] positions;
    [SerializeField] GameObject[] attackPositions;


    private string state;

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
                Debug.Log("Vital position: " + vital.transform.position);
                Debug.Log("player position: " + GameObject.FindGameObjectWithTag("Player").transform.position);
                Vector3 dir = GameObject.FindGameObjectWithTag("Player").transform.position - vital.transform.position;
                ProjectPlayer(dir, projectionForce);
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

    public void ProjectPlayer(Vector3 dir, float force)
    {
        Rigidbody2D playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            Vector2 projectionForce = dir.normalized * force;
            projectionForce.y = 1f;
            playerRb.AddForce(projectionForce, ForceMode2D.Force);
        }
        else
        {
            Debug.LogWarning("Player Rigidbody2D not found!");
        }
    }

    void Die()
    {
        audioManager.PlayMusic("track1");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddExperiencePoints(experiencePoints);
        bossHealthBar.SetActive(false);
        Destroy(gameObject);

    }
}
