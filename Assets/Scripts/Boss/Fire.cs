using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private int damage = 2;
    [SerializeField] private float duration = 2f;
    private float startTime;
    [SerializeField] private GameObject fire;
    public GameObject left;
    public GameObject right;

    public GameObject maxLeft;
    public GameObject maxRight;

    private bool itsLit = false;

    private void Update()
    {
        if (itsLit)
        {
            if (Time.time - startTime >= duration)
            {
                Destroy(gameObject);
            }
        }
    }
    public void AnimationEnd()
    {
        if (left == null && transform.position.x - 1 > maxLeft.transform.position.x)
        {
            left = Instantiate(fire);
            left.transform.position = transform.position;
            left.transform.position -= new Vector3(1, 0);
            left.GetComponent<Fire>().right = gameObject;
            left.GetComponent<Fire>().maxLeft = maxLeft;
            left.GetComponent<Fire>().maxRight = maxRight;
        }
        if (right == null && transform.position.x + 1 < maxRight.transform.position.x)
        {
            right = Instantiate(fire);
            right.transform.position = transform.position;
            right.transform.position += new Vector3(1, 0);
            right.GetComponent<Fire>().left = gameObject;
            right.GetComponent<Fire>().maxLeft = maxLeft;
            right.GetComponent<Fire>().maxRight = maxRight;
        }
        itsLit = true;
        startTime = Time.time;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (itsLit && other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(damage);

                Vector2 dir = Vector2.up;
                playerRb.velocity = Vector2.zero;
                Vector2 projectionForce = dir.normalized * 500;
                projectionForce.y = 1f;
                playerRb.AddForce(projectionForce, ForceMode2D.Force);
            }
            else
            {
                Debug.LogWarning("Player Rigidbody2D not found!");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (itsLit && other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(damage);

                Vector2 dir = Vector2.up;
                playerRb.velocity = Vector2.zero;
                Vector2 projectionForce = dir.normalized * 500;
                projectionForce.y = 1f;
                playerRb.AddForce(projectionForce, ForceMode2D.Force);
            }
            else
            {
                Debug.LogWarning("Player Rigidbody2D not found!");
            }
        }
    }
}
