using UnityEngine;
using UnityEngine.Tilemaps;

public class GrapplingHookBreakBlock : MonoBehaviour
{
    [SerializeField] private Tilemap destructableTilemap;
    [SerializeField] private Tilemap goldTilemap;
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private float destroyDistance;
    [SerializeField] private int defaultDurability = 3;
    [SerializeField] private int goldDurability = 5;

    private Collider2D grapplingHookCollider;

    private void Awake()
    {
        grapplingHookCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with the destructible tilemap
        if (collision.gameObject.layer == LayerMask.NameToLayer("Destructable"))
        {
            Vector3 hitPosition = collision.ClosestPoint(transform.position); // Grappling hook impact position
            Vector3Int tilePos = destructableTilemap.WorldToCell(hitPosition); // Get the tile position on the destructible tilemap

            // If the tile is found, handle durability and break the block
            if (destructableTilemap.HasTile(tilePos))
            {
                HandleDurability(destructableTilemap, tilePos, defaultDurability);
            }
        }
        // Check if the collision is with the gold tilemap
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Gold"))
        {
            Vector3 hitPosition = collision.ClosestPoint(transform.position); // Grappling hook impact position
            Vector3Int tilePos = goldTilemap.WorldToCell(hitPosition); // Get the tile position on the gold tilemap

            // If the gold tile is found, handle durability and break the block
            if (goldTilemap.HasTile(tilePos))
            {
                HandleDurability(goldTilemap, tilePos, goldDurability);
            }
        }
    }

    void HandleDurability(Tilemap targetTilemap, Vector3Int tilePos, int startingDurability)
    {
        int currentDurability = BlocksDurabilityManager.Instance.GetOrInitializeDurability(tilePos, startingDurability);

        // Use the BlocksDurabilityManager to reduce durability and update the tile
        BlocksDurabilityManager.Instance.ReduceDurability(tilePos, targetTilemap);

        // Check if the tile is destroyed after reducing durability
        if (currentDurability <= 0)
        {
            DestroyTile(targetTilemap, tilePos); // Destroy the tile if durability reaches 0
        }
    }

    void DestroyTile(Tilemap targetTilemap, Vector3Int tilePos)
    {
        // Set the tile to null, effectively destroying it
        targetTilemap.SetTile(tilePos, null);
    }

    private Vector3Int GetTileAtPosition(RaycastHit2D hit, Tilemap tilemap)
    {
        // Calculate the position of the tile based on the Raycast hit
        Vector3 hitPosition = Vector3.zero;
        hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
        hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
        Vector3Int cell = tilemap.WorldToCell(hitPosition);
        return cell;
    }
}