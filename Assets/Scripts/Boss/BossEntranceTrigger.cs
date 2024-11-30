using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntranceTrigger : MonoBehaviour
{
    [SerializeField] private BossMovement boss;
    [SerializeField] private GameObject bossHealthBar;

    void OnTriggerEnter2D(Collider2D other)
    {
        int layerMask = LayerMask.GetMask("Player");

        if (other.CompareTag("Player"))
        {
            boss.engaged = true;
            bossHealthBar.GetComponent<HealthBar>().Update_health(boss.health, boss.maxHealth);
            bossHealthBar.SetActive(true);
        }
    }
}
