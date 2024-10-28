using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PickaxeBreakBlock : MonoBehaviour
{
    public Tilemap tilemap;                    
    public GameObject highlightObject;         
    public Transform player;                  
    public float destroyDistance;   

    private Vector3Int tilePos;  
    private Vector3 tileWorldPos;   

    [SerializeField] private LayerMask layer;

    void Update()
    {
        // vai buscar posição do rato
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // tile no sitio do rato
        tilePos = tilemap.WorldToCell(mouseWorldPos);


        if (tilemap.HasTile(tilePos) && IsTileNearPlayer(tilePos))
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
        tileWorldPos = tilemap.CellToWorld(tilePos);


        highlightObject.transform.position = tileWorldPos + new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y / 2, 0); // Adjust for tile center
    }

    bool IsTileNearPlayer(Vector3Int tilePos)
    {
        tileWorldPos = tilemap.CellToWorld(tilePos);

        // Calculate direction from player to tile
        Vector2 direction = (tileWorldPos - player.position).normalized;

        // Perform Raycast
        RaycastHit2D hit = Physics2D.Raycast(player.position, direction, destroyDistance, layer);

        // Check if Raycast hit the target tile's position within range
        if (hit.collider != null)
        {
            Vector3Int hitTilePos = tilemap.WorldToCell(hit.point);

            Debug.Log("Player Position: " + player.position);
            Debug.Log("Hit Position: " + hit.point);
            Debug.Log("Tile Position: " + tilePos);

            // Check if the hit tile position matches the target tile
            return hitTilePos == tilePos;
        }

        return false;
    }
    
    // função chamada quando se quer partir bloco
    public void BreakBlock() {
            
        if (IsTileNearPlayer(tilePos))
        {
            tilemap.SetTile(tilePos, null);
        }
    }

 
}
