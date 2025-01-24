using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TurnLightOn : MonoBehaviour
{
    [SerializeField] GameObject light2D;
    [SerializeField] private float maxIntensity = 2;
    [SerializeField] private float fadeDuration = 2; 

    void Start()
    {
        light2D.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            light2D.SetActive(true);
            //StartCoroutine(FadeInLight());
            
        }
    }


}
