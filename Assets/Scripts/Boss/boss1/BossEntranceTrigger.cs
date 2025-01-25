using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntranceTrigger : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private GameObject bossHealthBar;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
            audioManager.PlayMusic("bossTrack");
            boss.engaged = true;
            bossHealthBar.GetComponent<HealthBar>().Update_health(boss.health, boss.maxHealth);
            bossHealthBar.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
