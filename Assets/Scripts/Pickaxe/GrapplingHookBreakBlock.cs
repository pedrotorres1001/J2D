using UnityEngine;
using UnityEngine.Tilemaps;

public class GrapplingHookBreakBlock : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;   
    [SerializeField] private Tilemap goldTilemap;
    [SerializeField] private GrapplingHook grapplingHook;
    public int defaultDurability = 3;
    public int goldDurability = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug the collision event to ensure it is firing
        Debug.Log("Grappling hook collided with: " + other.gameObject.name);

        Vector3 hitPosition = other.ClosestPoint(transform.position);
        Vector3Int tilePos = tilemap.WorldToCell(hitPosition);

        // Check if the tile position has a tile on either tilemap
        if (tilemap.HasTile(tilePos))
        {
            Debug.Log("Tile found on regular tilemap at position: " + tilePos);
            HandleDurability(tilemap, tilePos, defaultDurability);
        }
        else if (goldTilemap.HasTile(tilePos))
        {
            Debug.Log("Tile found on gold tilemap at position: " + tilePos);
            HandleDurability(goldTilemap, tilePos, goldDurability);
        }
        else
        {
            Debug.Log("No tile found at position: " + tilePos);
        }
    }

    void HandleDurability(Tilemap targetTilemap, Vector3Int tilePos, int startingDurability)
    {
        int currentDurability = BlocksDurabilityManager.Instance.GetOrInitializeDurability(tilePos, startingDurability);
        BlocksDurabilityManager.Instance.ReduceDurability(tilePos, targetTilemap);

        if (BlocksDurabilityManager.Instance.IsTileBroken(tilePos))
        {
            grapplingHook.RemoveGrapple();
            Debug.Log("Block broken by grappling hook at position: " + tilePos);
        }
        else
        {
            Debug.Log("Block hit by grappling hook! Remaining durability: " + currentDurability);
        }
    }
}