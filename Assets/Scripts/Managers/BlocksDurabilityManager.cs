using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlocksDurabilityManager : MonoBehaviour
{
    public static BlocksDurabilityManager Instance { get; private set; }

    // Define tile appearances for different durability levels
    [SerializeField] Tile goldDamaged;
    [SerializeField] Tile goldDamaged2;
    [SerializeField] Tile goldDamaged3;
    [SerializeField] Tile goldDamagedFull;
    [SerializeField] Tile stoneFull;
    [SerializeField] Tile stoneDamaged;
    [SerializeField] Tile stoneAlmostBroken;
    [SerializeField] int goldExperience;
    [SerializeField] GameObject dustEffectPrefab;
    [SerializeField] GameObject crystalPrefab;

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
                Vector3 worldPosition = tilemap.GetCellCenterWorld(tilePos); // Obter a posição do bloco
                InstantiateDustEffect(worldPosition); // Gerar as partículas no local do bloco

                tilemap.SetTile(tilePos, null);  // Remover o tile do tilemap
                tileDurabilities.Remove(tilePos); // Remover a durabilidade do bloco

                if (tilemap.gameObject.layer == 8)
                {
                    //player.GetComponent<Player>().AddExperiencePoints(goldExperience);
                    InstantiateCrystal(worldPosition); // Gerar as partículas no local do bloco
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

    private void InstantiateDustEffect(Vector3 worldPosition)
    {
        if (dustEffectPrefab != null)
        {
            Instantiate(dustEffectPrefab, worldPosition, Quaternion.identity);
        }
    }

    private void InstantiateCrystal(Vector3 worldPosition)
    {
        if(crystalPrefab != null)
        {
            Instantiate(crystalPrefab, worldPosition, Quaternion.identity);
        } 
    }
}