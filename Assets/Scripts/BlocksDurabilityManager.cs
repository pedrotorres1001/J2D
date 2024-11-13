using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DurabilityManager : MonoBehaviour
{
    public static DurabilityManager Instance { get; private set; }

    [SerializeField] private Tilemap tilemap;

    // Define tile appearances for different durability levels
    [SerializeField] private Tile tileStateFull;
    [SerializeField] private Tile tileStateDamaged;
    [SerializeField] private Tile tileStateAlmostBroken;

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

            // Update tile appearance based on new durability level
            UpdateTileAppearance(tilePos, tileDurabilities[tilePos]);

            // If the tile is broken, remove it from the dictionary and the tilemap
            if (tileDurabilities[tilePos] <= 0)
            {
                tilemap.SetTile(tilePos, null);  // Remove the tile from the tilemap
                tileDurabilities.Remove(tilePos); // Remove tile if broken
            }
        }
    }

    public bool IsTileBroken(Vector3Int tilePos)
    {
        return !tileDurabilities.ContainsKey(tilePos);
    }

    private void UpdateTileAppearance(Vector3Int tilePos, int currentDurability)
    {
        Tile newTile = null;

        // Choose the tile appearance based on durability
        if (currentDurability >= 3)
        {
            newTile = tileStateFull;  // Full durability
        }
        else if (currentDurability == 2)
        {
            newTile = tileStateDamaged;  // Medium durability
        }
        else if (currentDurability == 1)
        {
            newTile = tileStateAlmostBroken;  // Almost broken
        }

        // Update the tile on the tilemap
        if (newTile != null)
        {
            tilemap.SetTile(tilePos, newTile);
        }
    }
}