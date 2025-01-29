using System.Collections;
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
    [SerializeField] bool updateMap;
    [SerializeField] int mapLevel;

    [SerializeField] bool closeDoor;
    [SerializeField] GameObject door;

    private GrapplingHook grapplingHook;

    private void Start()
    {
        animator = GetComponent<Animator>();
        saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
        isLit = false;
        gameSceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameSceneManager>();
        firstSpawn = gameSceneManager.firstSpawn;
        grapplingHook = GameObject.FindGameObjectWithTag("Player").GetComponent<GrapplingHook>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (updateMap)
            {
                gameSceneManager.GetComponent<GameSceneManager>().currentLevel = mapLevel;
                PlayerPrefs.SetFloat("FirstRespawnX", gameObject.transform.position.x);
                PlayerPrefs.SetFloat("FirstRespawnY", gameObject.transform.position.y);
            }

            PlayerPrefs.SetFloat("RespawnX", gameObject.transform.position.x);
            PlayerPrefs.SetFloat("RespawnY", gameObject.transform.position.y);

            if (!isLit && animator != null)
            {
                animator.SetBool("isLit", true);
                isLit = true;
            }

            if(closeDoor && door != null)
            {
                door.SetActive(true);
            }

            // Inicia a corrotina para aguardar `isGrappling` ser true antes de gravar
            StartCoroutine(WaitForGrapplingAndSave());
        }
    }

    private IEnumerator WaitForGrapplingAndSave()
    {
        while (grapplingHook.isGrappling)
        {
            yield return null;
        }

        saveManager.SaveData(gameObject);
        textAnimator.SetTrigger("spawnText");
    }
}
