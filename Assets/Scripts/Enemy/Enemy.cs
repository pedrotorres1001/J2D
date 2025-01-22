using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    public int maxHealth = 100;
    private int currentHealth;
    public int CurrentHealth { get; }
    public int damage = 10;
    public GameObject explosionPrefab;

    public Animator animator;
    private Rigidbody2D rb;

    // Alterado para protected para ser acess�vel nas classes derivadas
    protected IEnemyState currentState;
    protected int direction = 1; // 1 para direita, -1 para esquerda

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

            // Atualiza a direção com base na velocidade
            if (velocity.x > 0)
            {
                direction = 1; 
            }
            else if (velocity.x < 0)
            {
                direction = -1;
            }

            // Atualiza o parâmetro no Animator
            animator?.SetFloat("Direction", direction);
        }
    }

    public Vector2 GetVelocity()
    {
        if (rb != null)
            return rb.velocity;        

        return Vector2.zero;   
    }

    public int GetDirection()
    {
        return direction;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        explosionPrefab.SetActive(true);
    }

    protected abstract void Update();
}

