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
    private Vector3 mouseWorldPos;
    private Vector3 tileWorldPos; 

    [SerializeField] private LayerMask layer;

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
        Debug.Log($"Player Position: {player.position}, Tile Position: {tileWorldPos}, Distance: {distanceToTile}");

        return distanceToTile <= destroyDistance;
    }
    
    // função chamada quando se quer partir bloco
    public void BreakBlock() {
            
        if (IsTileNearPlayer(tilePos))
        {
            tilemap.SetTile(tilePos, null);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position, destroyDistance); // Draw a radius around the player
    }

}
