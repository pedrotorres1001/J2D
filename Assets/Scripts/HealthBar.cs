using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    public void Update_health(float health, float maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }
}
