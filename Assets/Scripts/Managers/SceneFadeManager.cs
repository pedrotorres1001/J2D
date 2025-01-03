using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFadeManager : MonoBehaviour
{
    public Image fadeImage; // Drag the Image component here in the Inspector.
    public float fadeDuration = 1.0f;

    private void Start()
    {
        // Start with a fade-in effect
        //StartCoroutine(FadeInCR());
    }

    public void FadeIn() {
        StartCoroutine(FadeInCR());

    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCR());
    }

    private IEnumerator FadeInCR()
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = 1f - (timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }

    private IEnumerator FadeOutCR()
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = timer / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }
}