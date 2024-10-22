using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MonoBehaviour
{
    [SerializeField] private GameObject rightHand; // Posição da mão direita
    [SerializeField] private GameObject leftHand; // Posição da mão esquerda
    private PlayerMovement playerMovement; // Referência ao script de movimentação

    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>(); // Obtém a referência ao script de movimentação do jogador
    }

    void Update()
    {
        // Atualiza a posição da picareta com base na direção do jogador
        if (playerMovement.direction > 0)
        {
            transform.position = rightHand.transform.position; // Posição da mão direita
            transform.localScale = new Vector3(1, 1, 1); // Não espelha
        }
        else if (playerMovement.direction < 0)
        {
            transform.position = leftHand.transform.position; // Posição da mão esquerda
            transform.localScale = new Vector3(-1, 1, 1); // Espelha
        }
    }
}
