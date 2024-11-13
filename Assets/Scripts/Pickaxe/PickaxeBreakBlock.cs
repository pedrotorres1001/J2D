using UnityEngine;
using UnityEngine.Tilemaps;

public class PickaxeBreakBlock : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;   
    [SerializeField] private Tilemap goldTilemap;
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private Transform player;
    [SerializeField] private float destroyDistance;
    [SerializeField] private int defaultDurability = 3;
    [SerializeField] private int goldDurability = 5;
    [SerializeField] private Animator animator;

    private Vector3Int tilePos;
    private Vector3 tileWorldPos;
    private int direction; // 0 = Left, 1 = Right, 2 = Up, 3 = Down

    private PlayerMovement movement;

    private void Start() {
        movement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // Calcula as distâncias nos eixos X e Y entre o mouse e o jogador
        float distanceX = mouseWorldPos.x - player.position.x;
        float distanceY = mouseWorldPos.y - player.position.y;

        // Determina a direção principal baseada na distância dominante
        if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY))
        {
            // Horizontal (esquerda ou direita)
            direction = distanceX > 0 ? 1 : 0;
            int directionX = distanceX > 0 ? 1 : -1;
            tilePos = tilemap.WorldToCell(player.position + new Vector3(directionX * tilemap.cellSize.x, 0, 0));
        }
        else
        {
            // Vertical (cima ou baixo)
            direction = distanceY > 0 ? 2 : 3;
            int directionY = distanceY > 0 ? 1 : -1;
            tilePos = tilemap.WorldToCell(player.position + new Vector3(0, directionY * tilemap.cellSize.y, 0));
        }

        tileWorldPos = tilemap.GetCellCenterWorld(tilePos);

        // Verifica se o tile existe e se está dentro da distância de destruição
        if (tilemap.HasTile(tilePos) && IsTileNearPlayer(tileWorldPos))
        {
            //HighlightTile(tilePos);
        }
        else
        {
            //highlightObject.SetActive(false);
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
        return Vector3.Distance(player.position, tileWorldPos) <= destroyDistance;
    }

    public void BreakBlock()
    {
        if (IsTileNearPlayer(tilePos))
        {
            if (tilemap.HasTile(tilePos))
            {
                HandleDurability(tilemap, tilePos, defaultDurability);
            }
            else if (goldTilemap.HasTile(tilePos))
            {
                HandleDurability(goldTilemap, tilePos, goldDurability);
            }


            if(direction == 0) 
            {
                movement.lastDirection = -1;
            }
            else if(direction == 1)
            {
                movement.lastDirection = 1;
            }

            // Define o parâmetro Direction no Animator para tocar a animação correta
            //animator.SetTrigger("OnBreakBlock");
            //animator.SetTrigger("BreakBlock");
            //animator.SetInteger("Direction", direction);

        }
    }

    void HandleDurability(Tilemap targetTilemap, Vector3Int tilePos, int startingDurability)
    {
        int currentDurability = DurabilityManager.Instance.GetOrInitializeDurability(tilePos, startingDurability);
        DurabilityManager.Instance.ReduceDurability(tilePos);

        if (DurabilityManager.Instance.IsTileBroken(tilePos))
        {
            targetTilemap.SetTile(tilePos, null);
            Debug.Log("Block broken at position: " + tilePos);
        }
        else
        {
            Debug.Log("Block hit! Remaining durability: " + currentDurability);
        }
    }
}