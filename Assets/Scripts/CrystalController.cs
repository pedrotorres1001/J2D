using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 floatPosition;

    private int experience = 10;

    public float floatSpeed = 1f; // Velocidade da flutuação
    public float floatRange = 0.5f; // Distância para baixo a flutuar
    public bool canFloat;
    private GameObject player;
    private AudioManager audioManager;

    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        canFloat = false;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }


    void Update()
    {
        if(canFloat)
        {
            float t = Mathf.PingPong(Time.time * floatSpeed, 1f);
            transform.position = Vector3.Lerp(startPosition, floatPosition, t);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioManager.Play("pickUp");
            player.GetComponent<Player>().AddExperiencePoints(experience);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Destructable"))
        {
            startPosition = new Vector3(transform.position.x, transform.position.y + 0.1f); // Guardar posição inicial
            floatPosition = startPosition + Vector3.up * floatRange; // Calcular a posição ligeiramente abaixo
            canFloat = true;
        }
    }

}