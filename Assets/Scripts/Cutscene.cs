using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField] Animator irisOutAnimation; 
    [SerializeField] GameObject blackPanel;

    void Start()
    {
        blackPanel.SetActive(true);
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        blackPanel.SetActive(false);
        irisOutAnimation.SetTrigger("Open");
    }

}
