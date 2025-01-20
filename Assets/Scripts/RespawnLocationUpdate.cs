using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RespawnLocationUpdate : MonoBehaviour
{
    [SerializeField] bool isLit;
    private SaveManager saveManager;
    private GameSceneManager gameSceneManager;
    private Animator animator;
    [SerializeField] Animator textAnimator;
    private bool firstSpawn;

    private void Start()
    {
        animator = GetComponent<Animator>();
        saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
        isLit = false;

        firstSpawn = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameSceneManager>().firstSpawn;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("RespawnX", gameObject.transform.position.x);
            PlayerPrefs.SetFloat("RespawnY", gameObject.transform.position.y);


            if (firstSpawn == true)
            {
                GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameSceneManager>().firstSpawn = false;
                print("first spawn");
            }
            else
            {
                print("second spawn");
                saveManager.SaveData();
                textAnimator.SetTrigger("spawnText");
            }


            if (!isLit)
            {
                animator.SetBool("isLit", true);
                isLit = true;
            }
        }

    }


}
