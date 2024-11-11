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


    // Start is called before the first frame update
    void Start()
    {
        int playerMaxHealth = player.GetComponent<Player>().maxHealth;
        healthBar.maxValue = playerMaxHealth;
        //experienceBar.maxValue = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().maxExperience;

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = player.GetComponent<Player>().health;


    }
}
