using UnityEngine;
using UnityEngine.Tilemaps;

public class GrapplingHookBreakBlock : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap goldTilemap;
    [SerializeField] private GrapplingHook grapplingHook;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 hitPosition = other.ClosestPoint(transform.position);
        Vector3Int tilePos = tilemap.WorldToCell(hitPosition);

        if (tilemap.HasTile(tilePos))
        {
            BlocksDurabilityManager.Instance.InitializeTileDurability(tilePos, tilemap);
            BlocksDurabilityManager.Instance.ReduceDurability(tilePos);

            if (BlocksDurabilityManager.Instance.IsTileBroken(tilePos))
            {
                grapplingHook.RemoveGrapple();
            }
        }
    }
}