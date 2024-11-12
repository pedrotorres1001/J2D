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

    private Vector3Int tilePos;
    private Vector3 tileWorldPos;

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
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
        Vector3 centeredTilePos = tilemap.CellToWorld(tilePos) + new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y / 2, 0);
        highlightObject.transform.position = centeredTilePos;
    }

    bool IsTileNearPlayer(Vector3 tilePos)
    {
        return Vector3.Distance(player.position, tileWorldPos) <= destroyDistance;
    }

    public void BreakBlock()
    {
        if (IsTileNearPlayer(tilePos))
        {
            if (tilemap.HasTile(tilePos))
            {
                HandleDurability(tilemap, tilePos, defaultDurability);
            }
            else if (goldTilemap.HasTile(tilePos))
            {
                HandleDurability(goldTilemap, tilePos, goldDurability);
            }
        }
    }

    void HandleDurability(Tilemap targetTilemap, Vector3Int tilePos, int startingDurability)
    {
        int currentDurability = DurabilityManager.Instance.GetOrInitializeDurability(tilePos, startingDurability);
        DurabilityManager.Instance.ReduceDurability(tilePos);

        if (DurabilityManager.Instance.IsTileBroken(tilePos))
        {
            targetTilemap.SetTile(tilePos, null);
            Debug.Log("Block broken at position: " + tilePos);
        }
        else
        {
            Debug.Log("Block hit! Remaining durability: " + currentDurability);
        }
    }
}