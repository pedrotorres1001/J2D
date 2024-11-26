using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public UnityEngine.UI.Image health_bar;

    public void Update_health(float health, float maxHealth)
    {
        health_bar.fillAmount = health / maxHealth;
    }
}
