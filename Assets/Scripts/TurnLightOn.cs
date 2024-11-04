using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TurnLightOn : MonoBehaviour
{
    private Light2D light2D;
    [SerializeField] private float maxIntensity = 2;
    [SerializeField] private float fadeDuration = 2; 

    void Start()
    {
        light2D = gameObject.GetComponent<Light2D>();
        light2D.enabled = false;
        light2D.intensity = 0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            light2D.enabled = true;
            StartCoroutine(FadeInLight());      
        }
    }

    private IEnumerator FadeInLight()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            light2D.intensity = Mathf.Lerp(0f, maxIntensity, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        light2D.intensity = maxIntensity;
    }
}
