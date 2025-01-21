using UnityEngine;

public class AttackState : IEnemyState
{
    private Enemy enemy;
    private float attackRange;
    private float attackCooldown;
    private float attackDamage;
    private Transform target;

    public AttackState(float attackRange, float attackCooldown, float attackDamage, Transform target)
    {
        this.attackRange = attackRange;
        this.attackCooldown = attackCooldown;
        this.attackDamage = attackDamage;
        this.target = target;
    }

    public void EnterState(Enemy enemy)
    {
        this.enemy = enemy;
        // Aqui você pode iniciar a animação de ataque
    }

    public void UpdateState()
    {
        float distanceToPlayer = Vector2.Distance(enemy.transform.position, target.position);

        if (distanceToPlayer > attackRange)
        {
            enemy.SwitchState(new ChargeState(5f, 2f, attackRange, target)); // Volta para carga se estiver fora do alcance
        }

        // Lógica de ataque, por exemplo, verificar cooldown
        // Se o ataque for realizado com sucesso, adicione dano ao jogador, etc.
    }

    public void ExitState()
    {
        // Finalize o ataque, desative animação, etc.
    }
}
