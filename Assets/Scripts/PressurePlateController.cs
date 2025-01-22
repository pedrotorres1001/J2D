using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateController : MonoBehaviour
{
    public GameObject weightObject;
    public GameObject unlockableObject;

private void OnCollisionEnter2D(Collision2D other) {
    if(other.gameObject == weightObject)
        unlockableObject.SetActive(false);
    }
}
