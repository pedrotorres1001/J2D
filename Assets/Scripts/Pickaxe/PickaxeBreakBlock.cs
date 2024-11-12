using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PickaxeBreakBlock : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;   
    [SerializeField] private Tilemap goldTilemap;
    [SerializeField] private GameObject highlightObject;         
    [SerializeField] private Transform player;                  
    [SerializeField] private float destroyDistance;   
    [SerializeField] private int defaultDurability = 3;
    [SerializeField] private int goldDurability = 5;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask goldLayer;

    private Dictionary<Vector3Int, int> tileDurabilities = new Dictionary<Vector3Int, int>();
    private Vector3Int tilePos;  
    private Vector3 mouseWorldPos;
    private Vector3 tileWorldPos; 


    void Update()
    {
        // vai buscar posição do rato
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // tile no sitio do rato
        tilePos = tilemap.WorldToCell(mouseWorldPos);

        tileWorldPos = tilemap.GetCellCenterWorld(tilePos);

        if (tilemap.HasTile(tilePos) && IsTileNearPlayer(tileWorldPos))
        {
            HighlightTile(tilePos);    
        }
        else
        {
            highlightObject.SetActive(false);
        }
    }

    void HighlightTile(Vector3Int tilePos)
    {
        highlightObject.SetActive(true);

        // Get the tile's world position
        Vector3 tileWorldPos = tilemap.CellToWorld(tilePos);

        // Offset by half of the tile size to center the highlight
        Vector3 centeredTilePos = tileWorldPos + new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y / 2, 0);

        highlightObject.transform.position = centeredTilePos;    
    }

    bool IsTileNearPlayer(Vector3 tilePos)
    {
        float distanceToTile = Vector3.Distance(player.position, tileWorldPos);
        return distanceToTile <= destroyDistance;
    }
    
    // função chamada quando se quer partir bloco
    public void BreakBlock() {
            
        if (IsTileNearPlayer(tilePos))
        {
            // Check if tile exists in the primary tilemap layer
            if (tilemap.HasTile(tilePos))
            {
                HandleDurability(tilemap, tilePos, defaultDurability);
            }
            else if (goldTilemap.HasTile(tilePos))
            {
                HandleDurability(goldTilemap, tilePos, defaultDurability);
            }
        }
    }

    void HandleDurability(Tilemap targetTilemap, Vector3Int position, int startingDurability)
    {
        // Initialize durability if tile is hit for the first time
        if (!tileDurabilities.ContainsKey(position))
        {
            tileDurabilities[position] = startingDurability;
        }

        // Decrease durability
        tileDurabilities[position]--;

        // If durability reaches zero, break the block
        if (tileDurabilities[position] <= 0)
        {
            if(tilemap.HasTile(position))
            {
                targetTilemap.SetTile(position, null);
                tileDurabilities.Remove(position);
                Debug.Log("Block broken at position: " + position);
            } 
            else if(goldTilemap.HasTile(tilePos))
            {
                targetTilemap.SetTile(position, null);
                tileDurabilities.Remove(position);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddExperiencePoints(5);

                Debug.Log("Gold broken at position: " + position);
            }

        }
        else
        {
            Debug.Log("Block hit! Remaining durability: " + tileDurabilities[position]);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position, destroyDistance); // Draw a radius around the player
    }

}
