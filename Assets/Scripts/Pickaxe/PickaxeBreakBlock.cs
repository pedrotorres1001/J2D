using UnityEngine;
using UnityEngine.Tilemaps;

public class PickaxeBreakBlock : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap goldTilemap;
    [SerializeField] Tilemap rockTilemap;
    [SerializeField] GameObject highlightObject;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject player;
    [SerializeField] float destroyDistance = 2f; // Distance within which blocks can be destroyed
    [SerializeField] int defaultDurability = 3;
    [SerializeField] int goldDurability = 5;

    private Vector3Int tilePos;
    private Vector3 tileWorldPos;
    private Animator animator;

    private AudioManager audioManager;
    [SerializeField] private AudioSource SFXSource;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>(); 
    }

    private void Update()
    {
        // Get the mouse world position and snap it to the nearest tile
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // Ensure Z is zero for 2D games
        tilePos = tilemap.WorldToCell(mouseWorldPos); // Convert to tile position
        tileWorldPos = tilemap.GetCellCenterWorld(tilePos); // Snap to tile center

        // Highlight the tile if it's valid and near enough to the player
        if ((tilemap.HasTile(tilePos) || goldTilemap.HasTile(tilePos) || rockTilemap.HasTile(tilePos)) && IsTileNearPlayer(tileWorldPos))
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
        highlightObject.transform.position = tilemap.GetCellCenterWorld(tilePos); // Center the highlight object
    }

    bool IsTileNearPlayer(Vector3 tilePos)
    {
        // Check if the block is within the destroyDistance from the player
        return Vector3.Distance(player.transform.position, tilePos) <= destroyDistance;
    }

    public void BreakBlock()
    {
        ChangeDirection();

        // Break the block only if it's within the valid distance and facing the correct direction
        if (IsTileNearPlayer(tileWorldPos) && IsFacingCorrectDirection())
        {
            if (tilemap.HasTile(tilePos))
            {
                audioManager.Play("hitRock");
                HandleDurability(tilemap, tilePos, defaultDurability);
            }
            else if (goldTilemap.HasTile(tilePos))
            {
                audioManager.Play("hitRock");
                HandleDurability(goldTilemap, tilePos, goldDurability);
            }
            else if (rockTilemap.HasTile(tilePos))
            {
                audioManager.Play("hitRock");
                rockTilemap.SetTile(tilePos, null);
            }
        }
    }

    void ChangeDirection()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // Ensure Z is zero for 2D games

        // Calculate the vector from the player to the cursor
        Vector3 direction = mouseWorldPos - player.transform.position;

        // Calculate the angle between the player and the cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Determine the direction based on the angle
        if (angle > -45 && angle <= 45)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().lastDirection = 1;
        }
        else if (angle > 45 && angle <= 135)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().lastDirection = 3;
        }
        else if (angle > 135 || angle <= -135)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().lastDirection = -1;
        }
        else if (angle < -45 && angle >= -135)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().lastDirection = 4;
        }

        // Optional: Debug log to check the angle value
        Debug.Log("Angle: " + angle + ", LastDirection: " + animator.GetFloat("LastDirection"));
    }

    bool IsFacingCorrectDirection()
    {
        Vector3 directionToTile = (tileWorldPos - player.transform.position).normalized;
        float angle = Mathf.Atan2(directionToTile.y, directionToTile.x) * Mathf.Rad2Deg;
        float lastDirection = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().lastDirection;

        // Check if the player is facing the correct direction
        return (lastDirection == 1 && angle > -45 && angle <= 45) ||
               (lastDirection == 3 && angle > 45 && angle <= 135) ||
               (lastDirection == -1 && (angle > 135 || angle <= -135)) ||
               (lastDirection == 4 && angle < -45 && angle >= -135);
    }

    void HandleDurability(Tilemap targetTilemap, Vector3Int tilePos, int startingDurability)
    {
        int currentDurability = BlocksDurabilityManager.Instance.GetOrInitializeDurability(tilePos, startingDurability);

        // Use the BlocksDurabilityManager to reduce durability and update the tile appearance
        BlocksDurabilityManager.Instance.ReduceDurability(tilePos, targetTilemap);
    }
}