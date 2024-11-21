using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlocksDurabilityManager : MonoBehaviour
{
    public static BlocksDurabilityManager Instance { get; private set; }

    [SerializeField] private Tile goldDamaged, goldDamaged2, goldDamaged3, goldDamagedFull;
    [SerializeField] private Tile stoneFull, stoneDamaged, stoneAlmostBroken;

    private GameObject player;

    private Dictionary<Vector3Int, int> tileDurabilities = new Dictionary<Vector3Int, int>();

    // Default durability values by tilemap or layer
    private Dictionary<int, int> layerDurabilities = new Dictionary<int, int>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (Instance == null)
        {
            Instance = this;

            // Set default durability values for each layer
            layerDurabilities.Add(7, 3); // Stone layer
            layerDurabilities.Add(8, 5); // Gold layer
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetDurabilityForLayer(int layer)
    {
        return layerDurabilities.ContainsKey(layer) ? layerDurabilities[layer] : 1; // Default durability
    }

    public void InitializeTileDurability(Vector3Int tilePos, Tilemap tilemap)
    {
        if (!tilemap.HasTile(tilePos)) return; // Only initialize for valid tiles

        if (!tileDurabilities.ContainsKey(tilePos))
        {
            int layer = tilemap.gameObject.layer;
            int startingDurability = GetDurabilityForLayer(layer);
            tileDurabilities[tilePos] = startingDurability;
        }
    }

    public void ReduceDurability(Vector3Int tilePos)
    {
        if (!tileDurabilities.ContainsKey(tilePos)) return;

        tileDurabilities[tilePos]--;

        UpdateTileAppearance(tilePos, tileDurabilities[tilePos]);

        if (tileDurabilities[tilePos] <= 0)
        {
            Tilemap tilemap = FindTilemapForTile(tilePos);
            if (tilemap != null)
            {
                tilemap.SetTile(tilePos, null);
                tileDurabilities.Remove(tilePos);

                if (tilemap.gameObject.layer == 8) // Gold layer
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

    private void UpdateTileAppearance(Vector3Int tilePos, int currentDurability)
    {
        Tilemap tilemap = FindTilemapForTile(tilePos);
        if (tilemap == null) return;

        Tile newTile = null;

        int layer = tilemap.gameObject.layer;
        if (layer == 7) // Stone layer
        {
            if (currentDurability >= 3) newTile = stoneFull;
            else if (currentDurability == 2) newTile = stoneDamaged;
            else if (currentDurability == 1) newTile = stoneAlmostBroken;
        }
        else if (layer == 8) // Gold layer
        {
            if (currentDurability == 4) newTile = goldDamaged;
            else if (currentDurability == 3) newTile = goldDamaged2;
            else if (currentDurability == 2) newTile = goldDamaged3;
            else if (currentDurability == 1) newTile = goldDamagedFull;
        }

        if (newTile != null)
        {
            tilemap.SetTile(tilePos, newTile);
        }
    }

    private Tilemap FindTilemapForTile(Vector3Int tilePos)
    {
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.HasTile(tilePos)) return tilemap;
        }
        return null;
    }
}