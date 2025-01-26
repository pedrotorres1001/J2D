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

    private GameObject player;

    void Start()
    {
        startPosition = transform.position; // Guardar posição inicial
        floatPosition = startPosition + Vector3.down * floatRange; // Calcular a posição ligeiramente abaixo
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Flutuar entre a posição inicial e a posição ligeiramente abaixo
        float t = Mathf.PingPong(Time.time * floatSpeed, 1f);
        transform.position = Vector3.Lerp(startPosition, floatPosition, t);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<Player>().AddExperiencePoints(experience);
            Destroy(gameObject);
        }
    }
}