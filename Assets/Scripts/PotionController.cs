using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : MonoBehaviour
{
    [SerializeField] int potionHealth;
    public float floatSpeed = 1f; // Velocidade da flutuação
    public float floatRange = 0.5f; // Distância para baixo a flutuar
    private Vector3 startPosition;
    private Vector3 floatPosition;
    private AudioManager audioManager;

    private void Start() {
        startPosition = transform.position; // Guardar posição inicial
        floatPosition = startPosition + Vector3.down * floatRange; // Calcular a posição ligeiramente abaixo
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }

    private void Update() {
        float t = Mathf.PingPong(Time.time * floatSpeed, 1f);
        transform.position = Vector3.Lerp(startPosition, floatPosition, t);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Player>().health < collision.gameObject.GetComponent<Player>().maxHealth)
        {
            audioManager.Play("pickUp");
            collision.GetComponent<Player>().health += potionHealth;
            Destroy(gameObject);
        }
    }
}
