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
        // Verifica se a colisão foi com um tilemap destrutível
        if (collision.gameObject.layer == LayerMask.NameToLayer("Destructable"))
        {
            Vector3 hitPosition = collision.ClosestPoint(transform.position); // Posição de impacto do grappling hook
            Vector3Int tilePos = destructableTilemap.WorldToCell(hitPosition); // Posição do tile no tilemap

            // Se o tile for encontrado, reduce a durabilidade e quebra o bloco
            if (destructableTilemap.HasTile(tilePos))
            {
                HandleDurability(destructableTilemap, tilePos, defaultDurability);
            }
        }
        // Verifica se a colisão foi com o tilemap de ouro
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Gold"))
        {
            Vector3 hitPosition = collision.ClosestPoint(transform.position); // Posição de impacto do grappling hook
            Vector3Int tilePos = goldTilemap.WorldToCell(hitPosition); // Posição do tile no tilemap de ouro

            // Se o tile de ouro for encontrado, reduz a durabilidade e quebra o bloco
            if (goldTilemap.HasTile(tilePos))
            {
                HandleDurability(goldTilemap, tilePos, goldDurability);
            }
        }
    }

    void HandleDurability(Tilemap targetTilemap, Vector3Int tilePos, int startingDurability)
    {
        int currentDurability = BlocksDurabilityManager.Instance.GetOrInitializeDurability(tilePos, startingDurability);

        // Use o BlocksDurabilityManager para reduzir a durabilidade e atualizar o tile
        BlocksDurabilityManager.Instance.ReduceDurability(tilePos, targetTilemap);
    }
}