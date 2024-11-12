using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] private int damage;
    private void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.tag == "Player") 
        {
            Player script = other.gameObject.GetComponent<Player>();

            if(script != null) 
            {
                script.TakeDamage(damage);
            }
        }
    }
}
