using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    public float maxHealth = 100f;
    private float currentHealth;
    public float damage = 10f;

    public Animator animator;
    private Rigidbody2D rb;

    // Alterado para protected para ser acess�vel nas classes derivadas
    protected IEnemyState currentState;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        SwitchState(new PatrolState(2f));
    }

    public void SwitchState(IEnemyState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState(this);
    }

    public void SetVelocity(Vector2 velocity)
    {
        if (rb != null)
        {
            rb.velocity = velocity;
        }
    }

    public Vector2 GetVelocity()
    {
        if (rb != null)
            return rb.velocity;        

        return Vector2.zero;   
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // L�gica de morte
    }

    protected abstract void Update();
}

