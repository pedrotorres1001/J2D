using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverTimer : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] Sprite switchUsed;
    [SerializeField] Sprite switchUnused;
    [SerializeField] int time;
    [SerializeField] int damageWhenHit;
    private bool isColliding;
    private Transform player;
    private AudioManager audioManager;
    public BolderRespawn bolder;

    private void Start() {
        isColliding = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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
        audioManager.Play("ticking");

        // Ativa o switch e altera o sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = switchUsed;
        door.SetActive(false);

        // Espera o tempo definido
        yield return new WaitForSeconds(time);

        // Volta o sprite para switchUnused e fecha a porta
        gameObject.GetComponent<SpriteRenderer>().sprite = switchUnused;
        door.SetActive(true);



        if(player.position.y < door.transform.position.y)
        {
            player.position = transform.position;
            player.GetComponent<Player>().TakeDamage(damageWhenHit);
        }

        audioManager.Stop("ticking");
        bolder.ResetBolder();


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
