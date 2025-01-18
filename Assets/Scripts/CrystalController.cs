using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCrontroller : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasLanded = false;
    private Vector3 startPosition;
    private Vector3 groundPosition;

    public float floatSpeed = 1f; // Velocidade da flutuação
    private int experience = 15;

    private GameObject player;
 
     void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position; // Guardar posição inicial
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (hasLanded)
        {
            // Flutuar entre a posição inicial e a posição onde tocou no chão
            float t = Mathf.PingPong(Time.time * floatSpeed, 1f);
            transform.position = Vector3.Lerp(groundPosition, startPosition, t);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasLanded && (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Destructable")))
        {
            rb.bodyType = RigidbodyType2D.Static; // Tornar o cristal estático

            // Guardar a posição onde colidiu com o chão
            groundPosition = transform.position;

            // Parar o movimento do cristal
            rb.velocity = Vector2.zero;

            hasLanded = true;
        }
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
