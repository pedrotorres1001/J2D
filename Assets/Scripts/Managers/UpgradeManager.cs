using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeMenu;
    public Player player;
    public Button upgradeAttackDamageButton;
    public Button upgradeAttackSpeedButton;
    public GameObject pickaxe;
    private PickaxeAttack pickaxeAttack;
    private PickaxeController pickaxeController;

    void Start()
    {
        upgradeAttackDamageButton.onClick.AddListener(UpgradeAttackDamage);
        upgradeAttackSpeedButton.onClick.AddListener(UpgradeAttackSpeed);
        pickaxeAttack = pickaxe.GetComponent<PickaxeAttack>();
        pickaxeController = pickaxe.GetComponent<PickaxeController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }

        if (player != null && player.experience >= player.getMaxExperience())
        {
            upgradeMenu.SetActive(true);
        }
        else
        {
            upgradeMenu.SetActive(false);
        }
    }

    public void UpgradeAttackDamage()
    {
        if (player != null && player.experience >= player.getMaxExperience())
        {
            pickaxeAttack.attackDamage += 10;
            player.experience = 0;
            Debug.Log("Attack Damage Upgraded!");
        }
    }

    public void UpgradeAttackSpeed()
    {
        if (player != null && player.experience >= player.getMaxExperience())
        {
            player.experience = 0;
            pickaxeController.attackSpeed -= 0.1f;
            Debug.Log("Attack Speed Upgraded!");
        }
    }
}