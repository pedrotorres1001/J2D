using System.Collections;
using UnityEngine;

public class Boar : Enemy
{
    [Header("Boar Settings")]
    public float patrolSpeed = 2f;
    public float maxPatrolTime = 5f;
    public float chargeSpeed = 5f;
    public float chargeDuration = 2f;
    public float attackRange = 5f;
    public int chargeRange = 3;
    public float attackCooldown = 1.5f;
    public float attackDamage = 10f;
    private float sightRange = 4f;
    private float knockbackForce = 5f;
    private bool isCollidingWithPlayer = false;

    public Transform player;

    protected override void Start()
    {
        base.Start();
        SwitchState(new PatrolState(patrolSpeed));
    }

    public bool IsPlayerInFront()
    {
        // Define o alcance do Raycast
        float boxcastRange = sightRange;
        float boxWidth = 7f;
        float boxHeight = 5f;

        // Direção do raio: o Boar está olhando para a direita ou esquerda
        Vector2 direction = new Vector2(GetDirection(), 0);

        // Realiza o Raycast para verificar se o jogador está à frente
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(boxWidth, boxHeight), 0f, direction, boxcastRange, LayerMask.GetMask("Player"));

        // Se o Raycast acertar o jogador, o jogador está visível
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }

    public void TriggerChargeState()
    {
        SwitchState(new ChargeState(chargeSpeed, attackRange, player));
    }

    protected override void Update()
    {
        currentState?.UpdateState();
    }

    private void OnDrawGizmos()
    {
        if (transform != null)
        {
            Gizmos.color = Color.green; // Cor para a área de visão
            Vector2 direction = new Vector2(GetDirection(), 0); // Direção do BoxCast
            Vector2 boxSize = new Vector2(7f, 5f); // Tamanho da caixa (ajuste conforme necessário)

            // Desenha a caixa no editor para depuração
            Gizmos.DrawWireCube(transform.position + (Vector3)(direction * sightRange), boxSize);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Boar hit the player!");
            isCollidingWithPlayer = true;
            animator?.SetBool("isCharging", false);

            // Aqui, entra no estado de knockback após a colisão
            SwitchState(new KnockbackState(player, knockbackForce));
        }
    }

    private void OnCollisionExit2D(Collision2D collision) 
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Boar hit the player!");
            isCollidingWithPlayer = false;
        }
        
    }

}
