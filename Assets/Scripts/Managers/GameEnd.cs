using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{

    [SerializeField] Animator irisOutAnimation; 
    [SerializeField] GameObject blackPanel;


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Animation());
        }
    }

    private IEnumerator Animation()
    {
        irisOutAnimation.SetTrigger("Close");
        yield return new WaitForSeconds(.8f);
        blackPanel.SetActive(true);
        SceneManager.LoadScene("Credits");

    }
}
