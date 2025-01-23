using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [SerializeField] Animator irisOutAnimation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PlayAnimationAndChangeScene());
        }
    }

    private IEnumerator PlayAnimationAndChangeScene()
    {
        // Trigger da animação
        irisOutAnimation.SetTrigger("Close");

        // Espera até a animação acabar
        AnimatorStateInfo animationState = irisOutAnimation.GetCurrentAnimatorStateInfo(0);
        while (animationState.normalizedTime < 1 || !animationState.IsName("Close"))
        {
            yield return null;
            animationState = irisOutAnimation.GetCurrentAnimatorStateInfo(0); // Atualiza o estado
        }

        // Troca de cena
        SceneManager.LoadScene("Game");
    }
}