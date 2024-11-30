using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackTrigger : MonoBehaviour
{
    [SerializeField] private string[] ignoredTags; // Tags to ignore
    [SerializeField] private BossMovement boss;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other object has a tag in the ignored list
        foreach (string tag in ignoredTags)
        {
            if (other.CompareTag(tag))
            {
                Debug.Log($"Ignored object: {other.name} with tag {other.tag}");
                return; // Exit early to ignore this object
            }
            Debug.Log($"did not ignore object: {other.name} with tag {other.tag}");
        }

        if (other.CompareTag("Player"))
        {
            player.GetComponent<Player>().TakeDamage(boss.damage);
            StartCoroutine(boss.ProjectPlayer());
        }

        boss.ChangeState("postdash");
        this.gameObject.SetActive(false);
    }
}
