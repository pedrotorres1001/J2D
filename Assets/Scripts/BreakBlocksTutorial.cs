using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlocksTutorial : MonoBehaviour
{
    [SerializeField] private GameObject mouseInteraction;
    [SerializeField] private GameObject blockHighlight;

    private void Update()
    {
        // Ensure blockHighlight is not null before accessing its properties
        if (blockHighlight != null && blockHighlight.activeSelf)
        {
            mouseInteraction.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                mouseInteraction.SetActive(false);
                Destroy(gameObject);
            }
        }
        else
        {
            mouseInteraction.SetActive(false);
        }
    }
}