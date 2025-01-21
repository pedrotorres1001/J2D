using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private float patrolSpeed;

    public PatrolState(float patrolSpeed)
    {
        this.patrolSpeed = patrolSpeed;
    }

    public void EnterState(Enemy enemy)
    {
        this.enemy = enemy;
        // Define a direção inicial como para a direita
        enemy.SetVelocity(Vector2.right * patrolSpeed);
    }

    public void UpdateState()
    {
        // Verifica se há obstáculos
        CheckForObstacles();

        // Atualiza a velocidade para garantir que o inimigo continue se movendo
        Vector2 currentDirection = enemy.GetVelocity().normalized;
        enemy.SetVelocity(currentDirection * patrolSpeed);

        // Verifica se deve trocar de estado ao encontrar o jogador
        Boar boar = enemy as Boar;
        if (boar != null && boar.IsPlayerInFront())
        {
            float distanceToPlayer = Vector2.Distance(enemy.transform.position, boar.player.position);

            if (distanceToPlayer > boar.chargeRange)
                enemy.SwitchState(new ChargeState(boar.chargeSpeed, boar.chargeDuration, boar.attackRange, boar.player));
            else
                enemy.SwitchState(new AttackState(boar.attackRange, boar.attackCooldown, boar.attackDamage, boar.player));
        }
    }

    public void ExitState()
    {
        enemy.SetVelocity(Vector2.zero); // Para o movimento ao sair do estado
    }

    private void CheckForObstacles()
    {
        int layer = LayerMask.GetMask("Ground", "Destructable");
        float rayLength = 2f; // Alcance curto para detecção
        Vector2 rayOrigin = enemy.transform.position;
        Vector2 rayDirection = enemy.GetVelocity().normalized; // Direção atual do inimigo

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, layer);

        if (hit.collider != null)
        {
            // Inverte a direção do movimento ao encontrar um obstáculo
            Vector2 reverseDirection = -rayDirection;
            enemy.SetVelocity(reverseDirection * patrolSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        if (enemy == null) return;

        float rayLength = 2f; // Alcance do raycast
        Vector2 rayDirection = enemy.GetVelocity().normalized; // Direção atual do inimigo

        // Desenha o raycast no editor para depuração
        Gizmos.color = Color.red;
        Gizmos.DrawLine(enemy.transform.position, enemy.transform.position + (Vector3)(rayDirection * rayLength));
    }
}