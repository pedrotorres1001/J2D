using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private string[] ignoredTags; // Tags to ignore
    private Vector3 direction;
    private int damage;
    private float speed;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other object has a tag in the ignored list
        foreach (string tag in ignoredTags)
        {
            if (other.CompareTag(tag))
            {
                //Debug.Log($"Ignored object: {other.name} with tag {other.tag}");
                return; // Exit early to ignore this object
            }
            //Debug.Log($"did not ignore object: {other.name} with tag {other.tag}");
        }

        audioManager.Play(gameObject.GetComponent<AudioSource>(), "splash");

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetValues(Vector3 dir, int dmg, float spd)
    {
        direction = dir;
        damage = dmg; 
        speed = spd;
    }
}
