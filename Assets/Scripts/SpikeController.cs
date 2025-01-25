using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    [SerializeField] float gravity;
    private bool collidedWithPickaxe;

    private void Start() {
        collidedWithPickaxe = false;
    }

    private void Update() 
    {
        // Define o alcance do Raycast
        float boxcastRange = 1;
        float boxWidth = 0.5f;
        float boxHeight = 9f;

        // Realiza o Raycast para verificar se o jogador está à frente
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(boxWidth, boxHeight), 0f, Vector2.left, boxcastRange, LayerMask.GetMask("Player"));

        // Se o Raycast acertar o jogador, o jogador está visível
        if (hit.collider != null && (hit.collider.CompareTag("Player") || collidedWithPickaxe))
        {
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = gravity;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
        else if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(5);
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Pickaxe"))
        {
            collidedWithPickaxe = true;
        }

    }

    private void OnDrawGizmos()
    {
        if (transform != null)
        {
            Gizmos.color = Color.green; // Cor para a área de visão
            Vector2 boxSize = new Vector2(.5f, 9f); // Tamanho da caixa (ajuste conforme necessário)

            // Desenha a caixa no editor para depuração
            Gizmos.DrawWireCube(transform.position + (Vector3)(Vector2.left * 1), boxSize);
        }
    }

}
