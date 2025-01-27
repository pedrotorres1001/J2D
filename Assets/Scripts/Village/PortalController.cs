using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [SerializeField] Animator irisOutAnimation;
    [SerializeField] GameObject blackPanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickaxe"))
        {
            StartCoroutine(PlayAnimationAndChangeScene());
        }
    }

    private IEnumerator PlayAnimationAndChangeScene()
    {
        // Carrega a cena de forma assíncrona, mas não a ativa ainda
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game");
        operation.allowSceneActivation = false;

        // Inicia a animação
        irisOutAnimation.SetTrigger("Close");

        // Aguarda a duração da animação
        yield return new WaitForSeconds(.8f);

        blackPanel.SetActive(true);
        // Ativa a cena após a animação terminar
        operation.allowSceneActivation = true;
    }



}