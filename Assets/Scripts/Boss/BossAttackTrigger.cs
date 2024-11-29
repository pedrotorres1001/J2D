using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackTrigger : MonoBehaviour
{
    [SerializeField] private string[] ignoredTags; // Tags to ignore
    [SerializeField] private BossMovement boss;

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
            boss.ProjectPlayer();

        boss.ChangeState("postdash");
        this.gameObject.SetActive(false);
    }
}
