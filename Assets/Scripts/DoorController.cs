using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] Sprite openedDoor;
    [SerializeField] Sprite closedDoor;
    private SpriteRenderer spriteRenderer;
    private AudioManager audioManager;

    private bool isOpen;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isOpen = false;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }

    void Update()
    {
        if(!isOpen) {
            spriteRenderer.sprite = closedDoor;
        }
        else if (isOpen) 
        {   
            Destroy(gameObject.GetComponent<Collider2D>());
            //spriteRenderer.sprite = openedDoor;
        }
    }

    public void OpenDoor() {
        isOpen = true;
        StartCoroutine(OpenDoorAnimation());
    }

    private IEnumerator OpenDoorAnimation()
    {
        camera.Follow = transform;
        yield return new WaitForSeconds(1.5f);
        audioManager.Play("door");
        spriteRenderer.sprite = openedDoor;
        yield return new WaitForSeconds(1.5f);
        camera.Follow = player.transform;
    }
}
