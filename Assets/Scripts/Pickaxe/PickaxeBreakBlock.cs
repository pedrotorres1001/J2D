using UnityEngine;
using UnityEngine.Tilemaps;

public class PickaxeBreakBlock : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap goldTilemap;
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private float destroyDistance = 2f; // Distance within which blocks can be destroyed
    [SerializeField] private int defaultDurability = 3;
    [SerializeField] private int goldDurability = 5;

    private Vector3Int tilePos;
    private Vector3 tileWorldPos;
    private Animator animator;
    private AudioManager audioManager;

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

        // Break the block only if it's within the valid distance
        if (IsTileNearPlayer(tileWorldPos))
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
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().lastDirection = 2;
        }
        else if (angle > 135 || angle <= -135)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().lastDirection = -1;
        }
        else if (angle < -45 && angle >= -135)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().lastDirection = 3;
        }

        // Optional: Debug log to check the angle value
        Debug.Log("Angle: " + angle + ", LastDirection: " + animator.GetFloat("LastDirection"));
    }

    void HandleDurability(Tilemap targetTilemap, Vector3Int tilePos, int startingDurability)
    {
        int currentDurability = BlocksDurabilityManager.Instance.GetOrInitializeDurability(tilePos, startingDurability);

        // Use the BlocksDurabilityManager to reduce durability and update the tile appearance
        BlocksDurabilityManager.Instance.ReduceDurability(tilePos, targetTilemap);
    }
}