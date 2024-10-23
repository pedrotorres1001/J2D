using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDestroy : MonoBehaviour
{
public Tilemap tilemap;                    
    public GameObject highlightObject;         
    public Transform player;                  
    public float destroyDistance = 1.5f;      

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector3Int tilePos = tilemap.WorldToCell(mouseWorldPos);

        if (tilemap.HasTile(tilePos) && IsTileNearPlayer(tilePos))
        {
            HighlightTile(tilePos);    

            if (Input.GetMouseButtonDown(0))
            {
                if (IsTileNearPlayer(tilePos))
                {
                    tilemap.SetTile(tilePos, null);
                }
            }
        }
        else
        {
            highlightObject.SetActive(false);
        }
    }

    void HighlightTile(Vector3Int tilePos)
    {
        highlightObject.SetActive(true);
        Vector3 tileWorldPos = tilemap.CellToWorld(tilePos);
        highlightObject.transform.position = tileWorldPos + new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y / 2, 0); // Adjust for tile center
    }

    bool IsTileNearPlayer(Vector3Int tilePos)
    {
        Vector3 tileWorldPos = tilemap.CellToWorld(tilePos);
        return Vector3.Distance(player.position, tileWorldPos) <= destroyDistance;
    }
}
