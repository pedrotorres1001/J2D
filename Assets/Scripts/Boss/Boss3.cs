using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

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

    public GameObject leftBoundary;
    public GameObject rightBoundary;

    [SerializeField] float projectionForce = 9800f;

    public GameObject projectileSpawnPoint;
    [SerializeField] private GameObject vital;

    public AudioManager audioManager;

    [SerializeField] Boss3Trigger[] rechargePositions;
    [SerializeField] Boss3Trigger[] attackPositions;

    [SerializeField] private GameObject fireBall;
    [SerializeField] private int fireBallDamage = 8;
    [SerializeField] private float fireBallSpeed = 6f;

    [SerializeField] private GameObject projectile;
    [SerializeField] private int projectileDamage = 8;
    [SerializeField] private float projectileSpeed = 6f;

    [SerializeField] private GameObject groundFire;
    [SerializeField] private GameObject fireStartPos;
    [SerializeField] private float stompCooldown = 3f;
    [SerializeField] private float lastStomp;

    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject artefact;


    private int numAttacks = 0;

    public string state;

    private Transform player;

    private bool animationStarted = false;
    private bool isRecharging = false;
    private bool isTeleporting = false;
    private Vector3 targetPosition;

    private float rechargeStart;
    [SerializeField] private float rechargeTime = 4;

    [SerializeField] Transform startPosition;
    [SerializeField] GameObject entranceTrigger;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        state = "fireball";

    }

    private void Update()
    {
        if(player.GetComponent<Player>().health <= 0)
        {
            ResetBoss();
            return;
        }

        switch (engaged)
        {
            case true:

                switch (state)
                {
                    case "fireball":
                        if (!animationStarted)
                        {
                            animationStarted = true;
                            GetComponent<Animator>().Play("FinalBossFireBall", -1, 0f); 
                        }
                        break;
                    case "projectile":
                        if (!animationStarted)
                        { 
                            animationStarted = true;
                            GetComponent<Animator>().Play("FinalBossMultipleFire", -1, 0f);
                        }
                        break;
                    case "spike":
                        break;
                    case "flames":
                        if (!animationStarted)
                        { 
                            if (Time.time - lastStomp >= stompCooldown)
                            {
                                animationStarted = true;
                                GetComponent<Animator>().Play("FinalBossFireJump", -1, 0f);
                            }
                        }
                        break;
                    case "recharge":
                        if (Time.time - rechargeStart >= rechargeTime)
                        {
                            isRecharging = false;
                            numAttacks = 0;
                            state = "teleport";
                            vital.SetActive(false); 
                            ChooseNextPosition();
                        }
                        break;
                    case "teleport":
                        break;
                    case "repulse":
                        break;
                    default:
                        break;
                }
                break;
            case false:
                break;
        }
    }

    public void AnimationEnd()
    {
        if (numAttacks >= 4 && !isRecharging)
        {
            state = "teleport";
            vital.SetActive(false); 
            isRecharging = true;
            rechargeStart = Time.time;
            ChooseNextPosition();
            return;
        }

        switch (state)
        {
            case "fireball":
                numAttacks++;
                DecideAttack();

                break;
            case "projectile":
                numAttacks++;
                DecideAttack();

                break;
            case "spike":
                numAttacks++;
                DecideAttack();
                break;
            case "flames":
                numAttacks++;
                DecideAttack();
                break;
            case "recharge":
                break;
            case "teleport":
                if (isTeleporting)
                {
                    if (!isRecharging)
                    {
                        transform.position = targetPosition;
                        gameObject.GetComponent<Animator>().Play("FinalBossTeleportEnd", -1, 0f);
                    }
                    else
                    {
                        transform.position = targetPosition;
                        gameObject.GetComponent<Animator>().Play("FinalBossTeleportRechargeStart", -1, 0f);
                    }
                    isTeleporting = false;
                    vital.SetActive(true);
                }
                else
                {
                    if (isRecharging)
                    {
                        state = "recharge";
                    }
                    else
                    {
                        DecideAttack();
                    }
                }
                break;
            default:
                if (isRecharging)
                {
                    state = "recharge";
                }
                else
                {
                    if (isRecharging)
                    {
                        isRecharging = false;
                        numAttacks = 0;
                    }
                    state = "teleport";
                    ChooseNextPosition();
                }
                break;
        }
        
        animationStarted = false;
    }

    private void DecideAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float yDifference = Mathf.Abs(transform.position.y - 3f - player.position.y);


        if (yDifference <= 1.5f)
        {

            state = "flames";
        }
        else
        {
            if (UnityEngine.Random.Range(0, 10) < 5)
            {
                state = "fireball";
            }
            else
            {
                state = "projectile";
            }
        }
    }

    private void ChooseNextPosition()
    {
        if (isRecharging)
        {
            List<Boss3Trigger> targetPositions = new List<Boss3Trigger>();
            foreach (Boss3Trigger pos in rechargePositions)
            {
                if (!pos.isPlayerInArea)
                {
                    targetPositions.Add(pos);
                }
            }

            if (targetPositions.Count > 0) 
            {
                gameObject.GetComponent<Animator>().Play("FinalBossTeleportStart", -1, 0f);
                isTeleporting = true;
                targetPosition = targetPositions[UnityEngine.Random.Range(0, targetPositions.Count)].transform.position;
            }
        }
        else
        {
            List<Boss3Trigger> targetPositions = new List<Boss3Trigger>();
            foreach (Boss3Trigger pos in attackPositions)
            {
                if (!pos.isPlayerInArea)
                {
                    targetPositions.Add(pos);
                }
            }
            if (targetPositions.Count > 0)
            {
                gameObject.GetComponent<Animator>().Play("FinalBossTeleportStart", -1, 0f);
                isTeleporting = true;
                targetPosition = targetPositions[UnityEngine.Random.Range(0, targetPositions.Count)].transform.position;
            }
        }
    }

    public void CreateFireBall()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 dir = (player.transform.position - projectileSpawnPoint.transform.position).normalized;

        audioManager.Play("fireBall");
        GameObject proj = Instantiate(fireBall);
        proj.transform.position = projectileSpawnPoint.transform.position;
        proj.GetComponent<Projectile>().SetValues(dir, fireBallDamage, fireBallSpeed);
    }

    public void CreateProjectile()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 dir = (player.transform.position - projectileSpawnPoint.transform.position).normalized;

        audioManager.Play("fireProjectile");
        GameObject proj = Instantiate(projectile);
        proj.transform.position = projectileSpawnPoint.transform.position;
        proj.GetComponent<Projectile>().SetValues(dir, projectileDamage, projectileSpeed);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        proj.transform.Rotate(0, 0, 180);
    }

    public void CreateFire()
    {
        audioManager.Play("stomp");
        lastStomp = Time.time;
        GameObject fire = Instantiate(groundFire);
        fire.transform.position = fireStartPos.transform.position;

        fire.GetComponent<Fire>().maxLeft = leftBoundary;
        fire.GetComponent<Fire>().maxRight = rightBoundary;

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
                state = "repulse";
                gameObject.GetComponent<Animator>().Play("FinalBossRepulse", -1, 0f);
                hasShield = true;
                lastDamageTime = Time.time;
                Debug.Log("Vital position: " + vital.transform.position);
                Debug.Log("player position: " + GameObject.FindGameObjectWithTag("Player").transform.position);
                Vector3 dir = GameObject.FindGameObjectWithTag("Player").transform.position - vital.transform.position;
                ProjectPlayer(dir, projectionForce);
            }

            StartCoroutine(ColorChangeCoroutine());

            bossHealthBar.GetComponent<HealthBar>().Update_health(health, maxHealth);

            audioManager.Play(GetComponent<AudioSource>(), "enemyDeath");

            if (health <= 0)
            {
                Die();
            }
        }
    }

    public IEnumerator ColorChangeCoroutine()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
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

    public void ResetBoss()
    {
        state = "fireball";
        engaged = false;
        health = maxHealth;
        bossHealthBar.SetActive(false);
        entranceTrigger.SetActive(true);
        transform.position = startPosition.position;
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, transform.rotation);
        Instantiate(artefact, transform.position, transform.rotation);

        audioManager.PlayMusic("track1");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddExperiencePoints(experiencePoints);
        bossHealthBar.SetActive(false);
        Destroy(gameObject);

    }
}
