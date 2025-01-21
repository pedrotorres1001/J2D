using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookTrigger : MonoBehaviour
{
    public GrapplingHook grapplingHook;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Untagged") && !other.CompareTag("Lava") && !other.CompareTag("Confiner") && !other.CompareTag("Player"))
        {
            grapplingHook.hit = true;
        }
    }
}
