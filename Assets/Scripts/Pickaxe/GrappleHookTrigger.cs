using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookTrigger : MonoBehaviour
{
    private GrapplingHook grapplingHook;

    private void Start()
    {
        grapplingHook = GameObject.FindGameObjectWithTag("Player").GetComponent<GrapplingHook>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Untagged") && !other.CompareTag("Lava") && !other.CompareTag("Confiner") && !other.CompareTag("Player"))
        {
            grapplingHook.hit = true;
            if (other.CompareTag("Enemy"))
            {
                grapplingHook.grappleJoint.connectedBody = other.GetComponent<Rigidbody2D>();
                grapplingHook.grappleJoint.enabled = true;
            }
        }
    }
}
