using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyArtifact : MonoBehaviour
{
    public GameObject objectToDestroy;  
    public GameObject player;           
    public Transform teleportLocation;  
    
    [SerializeField] Animator irisOutAnimation;
    [SerializeField] GameObject blackPanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == objectToDestroy)
        {
            Destroy(objectToDestroy);

            if (player != null && teleportLocation != null)
            {
                player.transform.position = teleportLocation.position;
                GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>().PlayMusic("finalTrack");
            }
        }
                StartCoroutine(Wait());

            }
        }
    }

    private IEnumerator Wait()
    {
        irisOutAnimation.SetTrigger("Close");
        yield return new WaitForSeconds(.8f);
        blackPanel.SetActive(true);

        player.transform.position = teleportLocation.position;

        yield return new WaitForSeconds(1f);
        
        irisOutAnimation.SetTrigger("Open");
        blackPanel.SetActive(false);



    }
}
