using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeMenu;
    public GameObject player;
    public Button upgradeAttackDamageButton;
    public Button upgradeAttackSpeedButton;

    private Player playerComponent;
    private PickaxeAttack pickaxeAttack;
    private PickaxeController pickaxeController;

    void Start()
    {
        if (upgradeMenu != null)
        {
            upgradeMenu.SetActive(false);
        }

        if (upgradeAttackDamageButton != null)
        {
            upgradeAttackDamageButton.onClick.AddListener(UpgradeAttackDamage);
        }

        if (upgradeAttackSpeedButton != null)
        {
            upgradeAttackSpeedButton.onClick.AddListener(UpgradeAttackSpeed);
        }

        if (player != null)
        {
            playerComponent = player.GetComponent<Player>();
            pickaxeAttack = player.GetComponent<PickaxeAttack>();
            pickaxeController = player.GetComponent<PickaxeController>();
        }
    }

    void Update()
    {
        if (playerComponent != null && playerComponent.experience >= playerComponent.getMaxExperience())
        {
            upgradeMenu.SetActive(true);
        }
    }

    public void UpgradeAttackDamage()
    {
        if (pickaxeAttack != null)
        {
            pickaxeAttack.attackDamage += 1;
            playerComponent.experience = 0;
            Debug.Log("Attack Damage Upgraded");
            upgradeMenu.SetActive(false);
        }
    }

    public void UpgradeAttackSpeed()
    {
        if (pickaxeController != null)
        {
            pickaxeController.attackSpeed -= 0.1f;
            playerComponent.experience = 0;
            Debug.Log("Attack Speed Upgraded");
            upgradeMenu.SetActive(false);
        }
    }
}