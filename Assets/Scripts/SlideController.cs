using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            // other.gameObject.GetComponent<PlayerMovement>().slideEnabled = false;

            // other.gameObject.GetComponent<PlayerMovement>().groundCheckDistance = 2f;

            // if(other.gameObject.GetComponent<PlayerMovement>().isGrounded)
            // {
            //     other.gameObject.GetComponent<PlayerMovement>().Gravity = 30;
            //     other.gameObject.GetComponent<Animator>().SetBool("IsStairSliding", true);
            // }
            // else {
            //     other.gameObject.GetComponent<Animator>().SetBool("IsStairSliding", false);
            //     //other.gameObject.GetComponent<PlayerMovement>().Gravity = 10;
            // }

            other.gameObject.GetComponent<PlayerMovement>().enabled = false;
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 30;
        }

    }


    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            // other.gameObject.GetComponent<PlayerMovement>().isGrounded = false;

            // other.gameObject.GetComponent<PlayerMovement>().groundCheckDistance = 1f;

            other.gameObject.GetComponent<Animator>().SetBool("IsStairSliding", false);
            other.gameObject.GetComponent<PlayerMovement>().Gravity = 5;
            other.gameObject.GetComponent<PlayerMovement>().slideEnabled = true;
        }

    }
}
