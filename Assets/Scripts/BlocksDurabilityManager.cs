using System.Collections.Generic;
using UnityEngine;

public class DurabilityManager : MonoBehaviour
{
    public static DurabilityManager Instance { get; private set; }

    private Dictionary<Vector3Int, int> tileDurabilities = new Dictionary<Vector3Int, int>();

    private void Awake()
    {
        // Ensure there is only one instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetOrInitializeDurability(Vector3Int tilePos, int startingDurability)
    {
        if (!tileDurabilities.ContainsKey(tilePos))
        {
            tileDurabilities[tilePos] = startingDurability;
        }
        return tileDurabilities[tilePos];
    }

    public void ReduceDurability(Vector3Int tilePos)
    {
        if (tileDurabilities.ContainsKey(tilePos))
        {
            tileDurabilities[tilePos]--;

            if (tileDurabilities[tilePos] <= 0)
            {
                tileDurabilities.Remove(tilePos); // Remove tile if broken
            }
        }
    }

    public bool IsTileBroken(Vector3Int tilePos)
    {
        return !tileDurabilities.ContainsKey(tilePos);
    }
}