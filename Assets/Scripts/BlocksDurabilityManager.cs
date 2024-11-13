using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlocksDurabilityManager : MonoBehaviour
{
    public static BlocksDurabilityManager Instance { get; private set; }

    // Define tile appearances for different durability levels
    [SerializeField] private Tile goldDamaged;
    [SerializeField] private Tile goldDamaged2;
    [SerializeField] private Tile goldDamaged3;
    [SerializeField] private Tile goldDamagedFull;
    [SerializeField] private Tile stoneFull;
    [SerializeField] private Tile stoneDamaged;
    [SerializeField] private Tile stoneAlmostBroken;

    private GameObject player;

    private Dictionary<Vector3Int, int> tileDurabilities = new Dictionary<Vector3Int, int>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

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

    public void ReduceDurability(Vector3Int tilePos, Tilemap tilemap)
    {
        if (tileDurabilities.ContainsKey(tilePos))
        {
            tileDurabilities[tilePos]--;

            // Update tile appearance based on new durability level
            UpdateTileAppearance(tilePos, tileDurabilities[tilePos], tilemap);

            // If the tile is broken, remove it from the dictionary and the tilemap
            if (tileDurabilities[tilePos] <= 0)
            {
                tilemap.SetTile(tilePos, null);  // Remove the tile from the tilemap
                tileDurabilities.Remove(tilePos); // Remove tile if broken

                if(tilemap.gameObject.layer == 8)
                {
                    player.GetComponent<Player>().AddExperiencePoints(5);
                }
            }
        }
    }

    public bool IsTileBroken(Vector3Int tilePos)
    {
        return !tileDurabilities.ContainsKey(tilePos);
    }

    private void UpdateTileAppearance(Vector3Int tilePos, int currentDurability, Tilemap tilemap)
    {
        Tile newTile = null;

        if(tilemap.gameObject.layer == 7) 
        {
            // Choose the tile appearance based on durability
            if (currentDurability >= 3)
            {
                newTile = stoneFull;  // Full durability
            }
            else if (currentDurability == 2)
            {
                newTile = stoneDamaged;  // Medium durability
            }
            else if (currentDurability == 1)
            {
                newTile = stoneAlmostBroken;  // Almost broken
            }
        }
        else if (tilemap.gameObject.layer == 8)
        {
            // Choose the tile appearance based on durability
            if (currentDurability == 4)
            {
                newTile = goldDamaged;  // Full durability
            }
            else if (currentDurability == 3)
            {
                newTile = goldDamaged2;  // Medium durability
            }
            else if (currentDurability == 2)
            {
                newTile = goldDamaged3;  // Medium durability
            }
            else if (currentDurability == 1)
            {
                newTile = goldDamagedFull;  // Almost broken
            }
        }



        // Update the tile on the tilemap
        if (newTile != null)
        {
            tilemap.SetTile(tilePos, newTile);
        }
    }
}