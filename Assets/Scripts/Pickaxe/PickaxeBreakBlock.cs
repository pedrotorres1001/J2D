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

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
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
        Vector3 tileWorldPos = tilemap.CellToWorld(tilePos);

        Debug.Log("Player Position: " + player.position);
        Debug.Log("Tile Position: " + tileWorldPos);
        print(Vector3.Distance(player.position, tileWorldPos));

        return Vector3.Distance(player.position, tileWorldPos) <= destroyDistance;
    }
    
    public void BreakBlock() {
            
        if (IsTileNearPlayer(tilePos))
        {
            tilemap.SetTile(tilePos, null);
        }
    }

    void OnDrawGizmos()
    {
        // Draw player position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(player.position, 0.1f);  // Visualize the player's center

        // Draw the tile position
        Vector3 tileWorldPos = tilemap.CellToWorld(tilePos);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(tileWorldPos + new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y / 2, 0), 0.1f);  // Adjust to visualize the tile center
    }
}
