using UnityEngine;

public class KnockbackState : IEnemyState
{
    private Transform player;
    private float knockbackForce;

    public KnockbackState(Transform player, float knockbackForce)
    {
        this.player = player;
        this.knockbackForce = knockbackForce;
    }

    public void EnterState(Enemy enemy)
    {
        Debug.Log("Entered knockback state");

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // Calcular a direção do knockback
            Vector2 direction = (player.position - enemy.transform.position).normalized;

            // Aplica a força de knockback ao jogador
            playerRb.velocity = Vector2.zero; // Zera a velocidade antes de aplicar o knockback para garantir que não haja movimento residual
            playerRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Player does not have a Rigidbody2D!");
        }

        enemy.SetVelocity(Vector2.zero);
        enemy.SwitchState(new PatrolState(2f));

    }

    public void UpdateState()
    {
        // Nenhuma atualização necessária, já que o knockback é instantâneo
    }

    public void ExitState()
    {
        // Não há necessidade de limpar nada aqui, já que o knockback é instantâneo
        Debug.Log("Exited knockback state");
    }
}