using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakFloor : MonoBehaviour
{
    [SerializeField] GameObject dustPrefab;
    [SerializeField] Transform explosionPosition;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(dustPrefab, explosionPosition.transform.position, explosionPosition.transform.rotation);
            Destroy(gameObject);

            CameraShake cameraShake = FindObjectOfType<CameraShake>();
            if (cameraShake != null) // Certifique-se de que o script foi encontrado
            {
                cameraShake.Shake(2f, 0.5f); // Intensidade 2, duração 0.5s
            }
            else
            {
                Debug.LogWarning("CameraShake não encontrado na cena!");
            }
        }
    }
}