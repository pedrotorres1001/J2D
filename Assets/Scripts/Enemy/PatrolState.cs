using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private float patrolSpeed;
    private Vector2 patrolTarget;
    private float patrolTimer;
    private float maxPatrolTime;

    public PatrolState(float patrolSpeed, float maxPatrolTime)
    {
        this.patrolSpeed = patrolSpeed;
        this.maxPatrolTime = maxPatrolTime;
    }

    public void EnterState(Enemy enemy)
    {
        this.enemy = enemy;
        patrolTarget = GetRandomPatrolPoint();
        patrolTimer = maxPatrolTime;
        enemy.SetVelocity((patrolTarget - (Vector2)enemy.transform.position).normalized * patrolSpeed);
    }

    public void UpdateState()
    {
        patrolTimer -= Time.deltaTime;

        // Verificar se o inimigo encontrou algum obst�culo � frente usando um Raycast
        CheckForObstacles();

        if (patrolTimer <= 0 || Vector2.Distance(enemy.transform.position, patrolTarget) < 0.2f)
        {
            // Escolhe um novo ponto de patrulha
            patrolTarget = GetRandomPatrolPoint();
            enemy.SetVelocity((patrolTarget - (Vector2)enemy.transform.position).normalized * patrolSpeed);
        }

        // L�gica para verificar proximidade com o jogador e alternar de estado
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
        enemy.SetVelocity(Vector2.zero); // Para de se mover ao sair do estado
    }

    private Vector2 GetRandomPatrolPoint()
    {
        // Gera um ponto aleat�rio para patrulhar
        return (Vector2)enemy.transform.position + Random.insideUnitCircle * 3f;
    }

    // Fun��o para verificar se h� obst�culos � frente
    private void CheckForObstacles()
    {
        float rayLength = 1f; // Dist�ncia do raycast para verificar obst�culos
        Vector2 rayOrigin = (Vector2)enemy.transform.position + Vector2.up * 0.5f; // Inicia o raycast um pouco acima do ch�o
        Vector2 rayDirection = enemy.transform.right; // Dire��o do raio � para frente do inimigo

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, LayerMask.GetMask("Ground", "Destructable")); // Verifica no layer Ground ou Wall

        // Se o raycast colidir com algo (parede ou ch�o), mudar a dire��o
        if (hit.collider != null)
        {
            // Detec��o de obst�culo, vai para tr�s
            Vector2 reverseDirection = -rayDirection; // Dire��o oposta
            patrolTarget = (Vector2)enemy.transform.position + reverseDirection * 3f; // Muda o ponto de patrulha
            enemy.SetVelocity(reverseDirection * patrolSpeed); // Move para tr�s
        }
    }

}
