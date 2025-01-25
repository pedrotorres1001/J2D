using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolderRespawn : MonoBehaviour
{
    [SerializeField] private GameObject bolder; // Prefab do bolder
    [SerializeField] private Vector2 bolderLocation; // Localização do respawn

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Door"))
        {
            // Converte a posição para Vector3 e usa Quaternion.identity para rotação padrão
            Instantiate(bolder, new Vector3(bolderLocation.x, bolderLocation.y, 0), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}