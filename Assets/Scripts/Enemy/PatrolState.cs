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
        if(enemy is Boar boar)
        {
            if (boar.IsPlayerInFront()) // e nao tem nada a obstruir !!!
            {
                enemy.SwitchState(new ChargeState(boar.chargeSpeed, boar.attackRange, boar.player));
            }
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
}