using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchArrows : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform launchPoint;
    [SerializeField] float cooldown = 2f; 

    private float timer = 0f; 

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            timer += Time.deltaTime;

            if (timer >= cooldown) 
            {
                LaunchArrow();
                timer = 0f;
            }
        }
    }

    private void LaunchArrow()
    {
        if (arrowPrefab != null && launchPoint != null)
        {
            Instantiate(arrowPrefab, launchPoint.position, launchPoint.rotation);
        }

    }
}
