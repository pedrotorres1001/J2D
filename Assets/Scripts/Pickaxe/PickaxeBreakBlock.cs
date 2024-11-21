using UnityEngine;
using UnityEngine.Tilemaps;

public class PickaxeBreakBlock : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap goldTilemap;
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private GameObject player;
    [SerializeField] private float destroyDistance;
    [SerializeField] private Animator animator;

    private Vector3Int tilePos;
    private Vector3 tileWorldPos;
    private PlayerMovement movement;

    private void Start()
    {
        movement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Vector3 directionOffset = Vector3.zero;

        switch (movement.lastDirection)
        {
            case -1: directionOffset = new Vector3(-tilemap.cellSize.x, 0, 0); break;
            case 1: directionOffset = new Vector3(tilemap.cellSize.x, 0, 0); break;
            case 2: directionOffset = new Vector3(0, tilemap.cellSize.y, 0); break;
            case 3: directionOffset = new Vector3(0, -tilemap.cellSize.y, 0); break;
        }

        tilePos = tilemap.WorldToCell(player.transform.position + directionOffset);
        tileWorldPos = tilemap.GetCellCenterWorld(tilePos);

        // Check if the tilemap contains a tile at this position
        if (tilemap.HasTile(tilePos) && IsTileNearPlayer(tileWorldPos))
        {
            Debug.Log($"Tile at position {tilePos}: {tilemap.HasTile(tilePos)}");
            HighlightTile(tilePos);
        }
        else
        {
            highlightObject.SetActive(false); // Ensure the highlight is disabled
        }
    }

    void HighlightTile(Vector3Int tilePos)
    {
        if (!tilemap.HasTile(tilePos)) return; // Ensure there is a tile before highlighting
        highlightObject.SetActive(true);
        highlightObject.transform.position = tilemap.GetCellCenterWorld(tilePos);
    }
    bool IsTileNearPlayer(Vector3 tilePos)
    {
        return Vector3.Distance(player.transform.position, tileWorldPos) <= destroyDistance;
    }

    public void BreakBlock()
    {
        if (IsTileNearPlayer(tilePos))
        {
            BlocksDurabilityManager.Instance.InitializeTileDurability(tilePos, tilemap);
            BlocksDurabilityManager.Instance.ReduceDurability(tilePos);
        }
    }
}