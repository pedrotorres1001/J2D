using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCollider : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] int damage;
    private float lastAttackTime;


    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Knockback(other.gameObject);
                animator.SetTrigger("onJump");
                other.gameObject.GetComponent<Player>().TakeDamage(damage);
            
            }

            lastAttackTime = Time.time;
        }
    }

    private void Knockback(GameObject player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;

            // Determinar a direção horizontal com base na escala do inimigo
            float knockbackDirectionX = transform.localScale.x > 0 ? 1f : -1f; // 1 para a direita, -1 para a esquerda

            // Configurar a direção do knockback (diagonal)
            Vector2 knockbackDirection = new Vector2(-knockbackDirectionX, 1f).normalized; // Inclui a direção horizontal e vertical

            // Multiplicar pela força do knockback
            Vector2 projectionForce = knockbackDirection * 20f; // Ajusta a força conforme necessário

            playerRb.AddForce(projectionForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Player Rigidbody2D not found!");
        }
    }
}

