using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsTrigger : MonoBehaviour
{
    [SerializeField] GameObject stairsNormal;
    [SerializeField] GameObject stairsSlider;


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            stairsNormal.SetActive(false);
            stairsSlider.SetActive(true);
        }
    }
}
