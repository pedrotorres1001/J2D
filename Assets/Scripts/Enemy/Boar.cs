using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    // Variáveis públicas
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] public int attackDamage;

    // Variáveis privadas
    private float lastAttackTime;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private EnemyPatrol enemyPatrol;

    // Estado do Boar
    private enum State
    {
        Patrolling,
        ChasingPlayer,
        PreparingDash,
        Dashing,
        Attacking,
        Knockback
    }

    private State currentState;
    private Vector2 playerLastKnownPosition;

    private float knockDur = 1f;
    private float knockbackPwr = 5f;
    private bool isPerformingAction;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        enemyPatrol = GetComponent<EnemyPatrol>();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        currentState = State.Patrolling;  // Inicia o Boar patrulhando
        isPerformingAction = false;
    }

    private void Update()
    {
        if (isPerformingAction) return; // Ignorar lógica enquanto o Boar estiver a executar uma ação especial

        // Atualiza a posição do jogador
        playerLastKnownPosition = player.position;

        print(currentState);

        // Lógica de transição de estados
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;

            case State.ChasingPlayer:
                ChasePlayer();
                break;

            case State.PreparingDash:
                StartCoroutine(PrepareAndDash());
                break;

            case State.Dashing:
                // O Boar já está a "correr" no estado PrepareAndDash, então não faz nada aqui
                break;

            case State.Attacking:
                Attack();
                break;

            case State.Knockback:
                // Adicionar lógica para Knockback, se necessário
                break;
        }
    }

    private void Patrol()
    {
        // Inicia a patrulha
        bool canSeePlayer = HasLineOfSight();

        if (canSeePlayer)
        {
            currentState = State.PreparingDash;
            //currentState = State.ChasingPlayer;
        }
        else
        {
            enemyPatrol.Patrol(rb);
        }
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        Vector2 playerDirection = (player.position - transform.position).normalized;
        float direction = playerDirection.x > 0 ? 1 : -1; // Determina a direção
        animator.SetFloat("Direction", direction); // Atualiza animação
        rb.velocity = new Vector2(playerDirection.x * speed, 0); // Move apenas no eixo X

        // Se o Boar estiver a uma distância adequada, começa a preparar o dash
        if (distanceToPlayer < 3f)  // Distância para o dash
        {
            currentState = State.PreparingDash;
        }
    }

    private bool HasLineOfSight()
    {
        // BoxCast para detetar o jogador
        float boxcastRange = 7f;
        float boxWidth = 7f;
        float boxHeight = 5f;
        float direction = rb.velocity.x > 0 ? 1 : -1;

        LayerMask layerMask = LayerMask.GetMask("Player", "Ground", "Destructable");
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(boxWidth, boxHeight), 0f, Vector2.right * direction, boxcastRange, layerMask);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }

    private IEnumerator PrepareAndDash()
    {
        currentState = State.PreparingDash;
        isPerformingAction = true;

        // Preparação para o dash
        animator.SetBool("isPreparing", true);
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(1); // Espera antes de lançar o dash

        animator.SetBool("isPreparing", false);
        animator.SetBool("isCharging", true);

        // Realiza o dash
        Vector2 dashDirection = (playerLastKnownPosition - (Vector2)transform.position).normalized;
        float elapsedTime = 0f;
        float chargeAccelerationTime = 1f;

        while (elapsedTime < chargeAccelerationTime)
        {
            elapsedTime += Time.deltaTime;
            float currentSpeed = Mathf.Lerp(0, dashSpeed, elapsedTime / chargeAccelerationTime);
            rb.velocity = dashDirection * currentSpeed;
            yield return null;
        }

        rb.velocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration - chargeAccelerationTime);

        animator.SetBool("isCharging", false);
        currentState = State.Patrolling;  // Retorna ao estado de patrulha após o dash
        isPerformingAction = false;
    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, 1f);

            foreach (Collider2D target in hitPlayers)
            {
                if (target.CompareTag("Player"))
                {
                    target.GetComponent<Player>().TakeDamage(attackDamage);
                }
            }
            lastAttackTime = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentState = State.Knockback;
            StartCoroutine(Knockback());
        }
    }

    private IEnumerator Knockback()
    {
        float timer = 0;
        Rigidbody2D rbPlayer = player.GetComponent<Rigidbody2D>();

        while (knockDur > timer)
        {
            timer += Time.deltaTime;
            rbPlayer.AddForce(new Vector2(-knockbackPwr, knockbackPwr));  // Ajusta a força do knockback conforme necessário
        }

        currentState = State.Patrolling;  // Retorna à patrulha após o knockback
        yield return null;
    }

    private void OnDrawGizmos()
    {
        // Gizmos para visualizar a área de visão
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector2(7f, 5f));
    }
}
