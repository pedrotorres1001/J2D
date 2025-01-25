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
            if (other.CompareTag("Moss"))
            {
                grapplingHook.retractRope = true;
                grapplingHook.stopGrappling = true;
                return;
            }

            grapplingHook.hit = true;
            if (other.CompareTag("Enemy") || other.CompareTag("Chain"))
            {
                grapplingHook.grappleJoint.connectedBody = other.GetComponent<Rigidbody2D>();
                grapplingHook.grappleJoint.enabled = true;
            }
        }
    }
}
