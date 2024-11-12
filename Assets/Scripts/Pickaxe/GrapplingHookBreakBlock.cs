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
        Vector3 hitPosition = other.ClosestPoint(transform.position);
        Vector3Int tilePos = tilemap.WorldToCell(hitPosition);

        if (tilemap.HasTile(tilePos))
        {
            HandleDurability(tilemap, tilePos, defaultDurability);
        }
        else if (goldTilemap.HasTile(tilePos))
        {
            HandleDurability(goldTilemap, tilePos, goldDurability);
        }
    }

    void HandleDurability(Tilemap targetTilemap, Vector3Int tilePos, int startingDurability)
    {
        int currentDurability = DurabilityManager.Instance.GetOrInitializeDurability(tilePos, startingDurability);
        DurabilityManager.Instance.ReduceDurability(tilePos);

        if (DurabilityManager.Instance.IsTileBroken(tilePos))
        {
            targetTilemap.SetTile(tilePos, null);
            grapplingHook.RemoveGrapple();
            Debug.Log("Block broken by grappling hook at position: " + tilePos);
        }
        else
        {
            Debug.Log("Block hit by grappling hook! Remaining durability: " + currentDurability);
        }
    }
}