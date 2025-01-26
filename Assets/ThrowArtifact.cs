using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowArtifact : MonoBehaviour
{
    public GameObject objectToThrow; 
    public float throwForce = 25f;   

    private bool playerInside = false;
    private Transform player;
    private Rigidbody2D rb;

    private void Start()
    {
       
        if (objectToThrow != null)
        {
            rb = objectToThrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                
                rb.isKinematic = true;  
                rb.gravityScale = 0f;   
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            playerInside = true;
            player = other.transform;

            
            if (objectToThrow != null)
            {
                Vector3 objectWorldPosition = objectToThrow.transform.position;

                objectToThrow.transform.SetParent(player);

                objectToThrow.transform.position = objectWorldPosition;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;
        }
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F))
        {
            ThrowObject();
        }
    }

    private void ThrowObject()
    {
        if (objectToThrow == null || rb == null)
        {
            return;
        }

        objectToThrow.transform.SetParent(null); 

        rb.isKinematic = false; 
        rb.gravityScale = 1f;   

        rb.velocity = Vector2.zero; 
        rb.angularVelocity = 0f; 

        rb.AddForce(player.right * throwForce, ForceMode2D.Impulse); 
    }
}