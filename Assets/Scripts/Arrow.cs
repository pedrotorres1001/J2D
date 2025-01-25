using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Velocidade da flecha
    [SerializeField] private float lifetime = 5f; // Tempo até a flecha desaparecer
    [SerializeField] int damage;
    [SerializeField] Vector2 direction;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroi a flecha após 'lifetime' segundos
    }

    private void Update()
    {
        // Move a flecha para frente na sua direção atual
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {

            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject); // Destroi a flecha ao colidir
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destroi a flecha ao atingir uma parede
        }
    }
}
