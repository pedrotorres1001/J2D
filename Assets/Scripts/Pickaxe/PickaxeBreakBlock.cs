using UnityEngine;
using UnityEngine.Tilemaps;

public class PickaxeBreakBlock : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap goldTilemap;
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private GameObject player;
    [SerializeField] private float destroyDistance;
    [SerializeField] private int defaultDurability = 3;
    [SerializeField] private int goldDurability = 5;
    [SerializeField] private Animator animator;

    private Vector3Int tilePos;
    private Vector3 tileWorldPos;

    private PlayerMovement movement;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        movement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Determine tile position based on the last movement direction
        Vector3 directionOffset = Vector3.zero;

        switch (movement.lastDirection)
        {
            case -1: // Left
                directionOffset = new Vector3(-tilemap.cellSize.x, 0, 0);
                break;
            case 1: // Right
                directionOffset = new Vector3(tilemap.cellSize.x, 0, 0);
                break;
            case 2: // Up
                directionOffset = new Vector3(0, tilemap.cellSize.y, 0);
                break;
            case 3: // Down
                directionOffset = new Vector3(0, -tilemap.cellSize.y, 0);
                break;
        }

        tilePos = tilemap.WorldToCell(player.transform.position + directionOffset);
        tileWorldPos = tilemap.GetCellCenterWorld(tilePos);

        // Highlight tile if valid in either tilemap
        if ((tilemap.HasTile(tilePos) || goldTilemap.HasTile(tilePos)) && IsTileNearPlayer(tileWorldPos))
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
        return Vector3.Distance(player.transform.position, tileWorldPos) <= destroyDistance;
    }

    public void BreakBlock()
    {
        if (IsTileNearPlayer(tilePos))
        {
            if (tilemap.HasTile(tilePos))
            {
                HandleDurability(tilemap, tilePos, defaultDurability);
                audioManager.PlaySFX(audioManager.hitRock);
            }
            else if (goldTilemap.HasTile(tilePos))
            {
                HandleDurability(goldTilemap, tilePos, goldDurability);
                audioManager.PlaySFX(audioManager.hitRock);
            }
        }
    }

    void HandleDurability(Tilemap targetTilemap, Vector3Int tilePos, int startingDurability)
    {
        int currentDurability = BlocksDurabilityManager.Instance.GetOrInitializeDurability(tilePos, startingDurability);
        
        // Use the BlocksDurabilityManager to reduce durability and update the tile appearance
        BlocksDurabilityManager.Instance.ReduceDurability(tilePos, targetTilemap);
    }
    
}