using UnityEngine;

public class ChargeState : IEnemyState
{
    private Enemy enemy;
    private float chargeSpeed;
    private float attackRange;
    private Transform target;

    private bool isCharging;

    public ChargeState(float chargeSpeed, float attackRange, Transform target)
    {
        this.chargeSpeed = chargeSpeed;
        this.attackRange = attackRange;
        this.target = target;
    }

    public void EnterState(Enemy enemy)
    {
        Debug.Log("Entered charge state");
        this.enemy = enemy;

        // Calcula a direção para o jogador e inicia o charge
        Vector2 directionToTarget = (target.position - enemy.transform.position).normalized;
        enemy.SetVelocity(directionToTarget * chargeSpeed);

        // Ativa a animação de charge
        enemy.animator?.SetBool("isCharging", true);

        isCharging = true;
    }

    public void UpdateState()
    {
        if (!isCharging) return;

        // Verifica se o inimigo colidiu com um obstáculo à frente
        if (HasHitObstacle())
        {
            Debug.Log("Boar collided with an obstacle!");
            enemy.SwitchState(new PatrolState(2f));
        }
    }

    public void ExitState()
    {
        Debug.Log("Exited charge state");
        isCharging = false;

        // Para o movimento e reseta a animação
        enemy.SetVelocity(Vector2.zero);
        enemy.animator?.SetBool("isCharging", false);
    }

    private bool HasHitObstacle()
    {
        // Raycast para verificar colisões à frente
        float rayLength = 1f; // Ajuste conforme o tamanho do Boar
        Vector2 rayOrigin = enemy.transform.position;
        Vector2 rayDirection = enemy.GetVelocity().normalized;

        int layerMask = LayerMask.GetMask("Ground", "Destructable");

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(enemy.GetDirection(), 0), rayLength, layerMask);

        return hit.collider != null;
    }
}