using UnityEngine;
using UnityEngine.Tilemaps;

public class GrapplingHookBreakBlock : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap goldTilemap;
    [SerializeField] private GrapplingHook grapplingHook;
    public int defaultDurability = 3;
    public int goldDurability = 5;

    [SerializeField] private float collisionRadius = 1f;  // Radius to detect collision

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug the collision event to ensure it is firing
        Debug.Log("Grappling hook collided with: " + other.gameObject.name);

        // Get the position of the collision
        Vector3 hitPosition = other.ClosestPoint(transform.position);
        Vector3Int tilePos = tilemap.WorldToCell(hitPosition);

        // Perform a radius check for collision
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, collisionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Destructable") || hitCollider.CompareTag("Gold"))
            {
                Vector3Int currentTilePos = tilemap.WorldToCell(hitCollider.transform.position);
                // Handle tile breaking based on the map layer
                if (tilemap.HasTile(currentTilePos))
                {
                    HandleDurability(tilemap, currentTilePos, defaultDurability);
                }
                else if (goldTilemap.HasTile(currentTilePos))
                {
                    HandleDurability(goldTilemap, currentTilePos, goldDurability);
                }
            }
        }
    }

    void HandleDurability(Tilemap targetTilemap, Vector3Int tilePos, int startingDurability)
    {
        // Get or initialize the durability of the tile
        int currentDurability = BlocksDurabilityManager.Instance.GetOrInitializeDurability(tilePos, startingDurability);
        
        // Reduce the durability of the tile using the BlocksDurabilityManager
        BlocksDurabilityManager.Instance.ReduceDurability(tilePos, targetTilemap);

        // Check if the tile is broken
        if (BlocksDurabilityManager.Instance.IsTileBroken(tilePos))
        {
            grapplingHook.RemoveGrapple();
            Debug.Log("Block broken by grappling hook at position: " + tilePos);
        }
        else
        {
            // Log remaining durability
            Debug.Log("Block hit by grappling hook! Remaining durability: " + currentDurability);
        }
    }
}