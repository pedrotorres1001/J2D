using UnityEngine;

public class ChargeState : IEnemyState
{
    private Enemy enemy;
    private float chargeSpeed;
    private float chargeDuration;
    private float attackRange;
    private Transform target;
    private float chargeTimer;

    public ChargeState(float chargeSpeed, float chargeDuration, float attackRange, Transform target)
    {
        this.chargeSpeed = chargeSpeed;
        this.chargeDuration = chargeDuration;
        this.attackRange = attackRange;
        this.target = target;
    }

    public void EnterState(Enemy enemy)
    {
        this.enemy = enemy;
        chargeTimer = chargeDuration;

        Vector2 directionToTarget = (target.position - enemy.transform.position).normalized;
        enemy.SetVelocity(directionToTarget * chargeSpeed);
        enemy.animator.SetBool("isCharging", true);  // Usando GetAnimator() para acessar o Animator
    }

    public void UpdateState()
    {
        chargeTimer -= Time.deltaTime;

        if (Vector2.Distance(enemy.transform.position, target.position) <= attackRange)
        {
            enemy.SwitchState(new AttackState(attackRange, 1f, 10f, target));
            return;
        }

        if (chargeTimer <= 0)
        {
            enemy.SwitchState(new PatrolState(2f));
        }
    }

    public void ExitState()
    {
        enemy.SetVelocity(Vector2.zero);
        enemy.animator?.SetBool("isCharging", false);  // Usando GetAnimator() aqui tamb�m
    }
}

