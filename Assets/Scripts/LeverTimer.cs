using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverTimer : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] Sprite switchUsed;
    [SerializeField] Sprite switchUnused;
    [SerializeField] int time;
    private bool isColliding;

    private void Start() {
        isColliding = false;
    }

    private void Update() 
    {
        if(isColliding && Input.GetKeyDown(KeyCode.F)) 
        {
            if(door != null) 
            {

                StartCoroutine(ActivateSwitchWithTimer());

            }
        }
    }
    private IEnumerator ActivateSwitchWithTimer()
    {
        // Ativa o switch e altera o sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = switchUsed;
        door.SetActive(false);

        // Espera o tempo definido
        yield return new WaitForSeconds(time);

        // Volta o sprite para switchUnused e fecha a porta
        gameObject.GetComponent<SpriteRenderer>().sprite = switchUnused;
        door.SetActive(true);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) {
            isColliding = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) {
            isColliding = false;
        }
    }
}
