using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArtefactController : MonoBehaviour
{
    [SerializeField] Animator irisOutAnimation;
    [SerializeField] GameObject blackPanel;

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && Input.GetKey(KeyManager.KM.interact))
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        irisOutAnimation.SetTrigger("Close");
        yield return new WaitForSeconds(.8f);
        blackPanel.SetActive(true);
        SceneManager.LoadScene("Cutscene");
    }
}
