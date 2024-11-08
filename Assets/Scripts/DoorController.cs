using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] Sprite openedDoor;
    [SerializeField] Sprite closedDoor;
    [SerializeField] SpriteRenderer spriteRenderer;

    private bool isOpen;

    void Start()
    {
        isOpen = false;
    }

    void Update()
    {
        if(!isOpen) {
            spriteRenderer.sprite = closedDoor;
        }
        else if (isOpen) 
        {
            //spriteRenderer.sprite = openedDoor;
            Destroy(gameObject);
        }
    }

    public void OpenDoor() {
        isOpen = true;
    }
}
