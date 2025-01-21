using UnityEngine;

public class Boar : Enemy
{
    [Header("Boar Settings")]
    public float patrolSpeed = 2f;
    public float maxPatrolTime = 5f;
    public float chargeSpeed = 5f;
    public float chargeDuration = 2f;
    public float attackRange = 1.5f;
    public float chargeRange = 2.0f;
    public float attackCooldown = 1.5f;
    public float attackDamage = 10f;
    public float fieldOfViewAngle = 45f;
    private float sightRange = 5f;

    public Transform player; // Referência para o jogador

    protected override void Start()
    {
        base.Start();
        SwitchState(new PatrolState(patrolSpeed, maxPatrolTime));
    }

    public bool IsPlayerInFront()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        float angle = Vector2.Angle(transform.right, directionToPlayer);

        return angle < fieldOfViewAngle && Vector2.Distance(transform.position, player.position) <= sightRange;
    }

    public void TriggerChargeState()
    {
        SwitchState(new ChargeState(chargeSpeed, chargeDuration, attackRange, player));
    }

    public void TriggerAttackState()
    {
        SwitchState(new AttackState(attackRange, attackCooldown, attackDamage, player));
    }

    protected override void Update()
    {
        currentState?.UpdateState();
    }
}
