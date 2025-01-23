using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCollider : MonoBehaviour
{
    public bool canSeePlayer;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
            canSeePlayer = true;
    }

        private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
            canSeePlayer = false;
    }
}
