using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeMenu;
    private Player player;
    public GameObject pickaxe;
    private PickaxeAttack pickaxeAttack;
    private PickaxeController pickaxeController;

    void Start()
    {
        pickaxeAttack = pickaxe.GetComponent<PickaxeAttack>();
        pickaxeController = pickaxe.GetComponent<PickaxeController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void activateUpgradeMenu()
    {

        upgradeMenu.SetActive(true);

    }

    public void UpgradeAttackDamage()
    {
        pickaxeAttack.attackDamage += 5;
    }

    public void UpgradeAttackSpeed()
    { 
        pickaxeController.attackSpeed -= 0.02f;
    }
}