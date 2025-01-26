using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoss : MonoBehaviour
{
    [SerializeField] Animator lavaAnimator;
    [SerializeField] GameObject undergroundLava;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Grapple"))
        {
            undergroundLava.SetActive(true);
            lavaAnimator.SetTrigger("RemoveLava");
            Destroy(gameObject);
        }
    }
}
