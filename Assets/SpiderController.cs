using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f; // Velocidade de movimento
    public float moveHeight = 2f; // Distância vertical que a aranha percorre

    [Header("Damage Settings")]
    public int damage = 1; // Dano causado ao jogador

    private Vector3 startPosition; // Posição inicial da aranha
    private bool movingUp = true; // Direção atual do movimento

    public GameObject crystal;
    public ParticleSystem deathPrefab;
    public Transform deathPosition;

    void Start()
    {
        // Guarda a posição inicial
        startPosition = transform.position;
    }

    void Update()
    {
        // Calcula o movimento de subida e descida
        float newY = transform.position.y + (movingUp ? moveSpeed : -moveSpeed) * Time.deltaTime;

        // Verifica se chegou ao limite superior ou inferior
        if (newY >= startPosition.y + moveHeight)
        {
            newY = startPosition.y + moveHeight;
            movingUp = false; // Inverte a direção
        }
        else if (newY <= startPosition.y - moveHeight)
        {
            newY = startPosition.y - moveHeight;
            movingUp = true; // Inverte a direção
        }

        // Atualiza a posição
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o jogador colidiu com a aranha
        if (collision.CompareTag("Player"))
        {
            // Aqui você pode aplicar dano ao jogador
            Player playerHealth = collision.GetComponent<Player>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        if(collision.CompareTag("Pickaxe") || collision.CompareTag("Grapple"))
        {            
            deathPrefab.Play();
            Instantiate(crystal, deathPosition.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}