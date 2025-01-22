using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpikeFall : MonoBehaviour
{
    public Rigidbody2D spikeRB;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            spikeRB.bodyType = RigidbodyType2D.Dynamic;
            //spikeRB.GetComponent<Collider2D>().isTrigger = true;
        }
    }
}
