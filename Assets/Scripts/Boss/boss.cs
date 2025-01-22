using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject bossHealthBar;
    public int health = 800;
    public int maxHealth = 800;
    public int damage = 10;

    public int experiencePoints;

    public int attackDamage;

    // Movement properties
    public float speed;
    public Rigidbody2D rb;
    public bool engaged = false;

    public GameObject leftBoundary;
    public GameObject rightBoundary;
}
