using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider experienceBar;
    [SerializeField] private GameObject player;

    void Start()
    {
        int playerMaxHealth = player.GetComponent<Player>().maxHealth;
        healthBar.maxValue = playerMaxHealth;
    }

    void Update()
    {
        healthBar.value = player.GetComponent<Player>().health;
        experienceBar.value = player.GetComponent<Player>().experience;
    
    }

    // public void UpgradePickaxe()
    // {
    //     Player player = player.GetComponent<Player>();
    //     PickaxeAttack pickaxe = player.GetComponent<PickaxeAttack>();

    //     if (player.experience >= 100)
    //     {
    //         player.experience -= 100;
    //         pickaxe.attackDamage += 10;
    //         UpdateExperienceBar();
    //     }
    // }

    // public void UpdateExperienceBar()
    // {
    //     experienceBar.value = player.GetComponent<Player>().experience;
    // }
}