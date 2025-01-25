using UnityEngine;
using UnityEngine.Tilemaps;

public class PickaxeBreakBlock : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap goldTilemap;
    [SerializeField] GameObject highlightObject;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject player;
    [SerializeField] float destroyDistance = 2f;

    [SerializeField] int defaultDurability = 3; // Durability for normal blocks
    [SerializeField] int goldDurability = 5;   // Durability for gold blocks

    private Vector3Int tilePos;
    private Vector3 tileWorldPos;
    private Animator animator;

    private PlayerMovement playerMovement;

    // Variables for block durability
    private int currentDurability;
    private bool isBreaking;

    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        animator = player.GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        tilePos = tilemap.WorldToCell(mouseWorldPos);
        tileWorldPos = tilemap.GetCellCenterWorld(tilePos);

        if ((tilemap.HasTile(tilePos) || goldTilemap.HasTile(tilePos)) && IsTileNearPlayer())
        {
            HighlightTile();
        }
        else
        {
            highlightObject.SetActive(false);
        }
    }

    void HighlightTile()
    {
        highlightObject.SetActive(true);
        highlightObject.transform.position = tileWorldPos;
    }

    bool IsTileNearPlayer()
    {
        return Vector3.Distance(player.transform.position, tileWorldPos) <= destroyDistance;
    }

    public void BreakBlock()
    {
        UpdatePlayerDirection();

        if (IsTileNearPlayer() && IsFacingCorrectDirection())
        {
            // Check which tilemap contains the tile and initialize durability
            if (tilemap.HasTile(tilePos))
            {
                StartBreaking(tilePos, tilemap, defaultDurability);
            }
            else if (goldTilemap.HasTile(tilePos))
            {
                StartBreaking(tilePos, goldTilemap, goldDurability);
            }
        }
    }

    void StartBreaking(Vector3Int position, Tilemap map, int maxDurability)
    {
        if (!isBreaking)
        {
            currentDurability = maxDurability;
            isBreaking = true;
            animator.SetBool("IsBreaking", true); // Start breaking animation
        }

        // Reduce durability
        currentDurability--;

        if (currentDurability <= 0)
        {
            // Break the block
            map.SetTile(position, null);
            isBreaking = false;
            animator.SetBool("IsBreaking", false); // Stop breaking animation
        }
    }

    void UpdatePlayerDirection()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3 direction = (mouseWorldPos - player.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle > -45 && angle <= 45)
        {
            playerMovement.lastDirection = 1; // Right
        }
        else if (angle > 45 && angle <= 135)
        {
            playerMovement.lastDirection = 3; // Up
        }
        else if (angle > 135 || angle <= -135)
        {
            playerMovement.lastDirection = -1; // Left
        }
        else if (angle < -45 && angle >= -135)
        {
            playerMovement.lastDirection = 4; // Down
        }

        animator.SetFloat("LastDirection", playerMovement.lastDirection);
    }

    bool IsFacingCorrectDirection()
    {
        Vector3 directionToTile = (tileWorldPos - player.transform.position).normalized;
        float angle = Mathf.Atan2(directionToTile.y, directionToTile.x) * Mathf.Rad2Deg;

        return (playerMovement.lastDirection == 1 && angle > -45 && angle <= 45) ||
               (playerMovement.lastDirection == 3 && angle > 45 && angle <= 135) ||
               (playerMovement.lastDirection == -1 && (angle > 135 || angle <= -135)) ||
               (playerMovement.lastDirection == 4 && angle < -45 && angle >= -135);
    }
}